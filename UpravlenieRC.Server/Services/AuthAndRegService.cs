using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;
namespace UpravlenieRC.Server.Services
{
    public class AuthAndRegService
    {
        private readonly PasswordHasher<User> hasher = new PasswordHasher<User>();
        private readonly RC_SkladContext _context;
        private int minPasswordLength = 4;
        private int minLoginLength = 4;
        public AuthAndRegService(RC_SkladContext context)
        {
            _context = context;
        }
        public async Task<ResponseWithMessage> Reg(RegRequest request)
        {
            
            if (string.IsNullOrEmpty(request.INN) || string.IsNullOrEmpty(request.message))
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Сообщение и ИНН не могут быть пустыми");
            }
            if (request.Password.Length < minPasswordLength)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, $"Пароль должен быть длинее {minPasswordLength} символов");
            }
            if (request.Login.Length < minLoginLength)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, $"Логин должен быть длинее {minLoginLength} символов");
            }
            if ((await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login)) != null || (await _context.RegestrationRequests.FirstOrDefaultAsync(u => u.Login == request.Login)) != null)
            {
                return new ResponseWithMessage(HttpStatusCode.Conflict, "Пользователь с таким логином уже существует");
            }
            Partner? partner = await _context.Partners.FirstOrDefaultAsync(p => p.Iin == request.INN);
            if (partner == null)
            {
                return new ResponseWithMessage(HttpStatusCode.NotFound, "Организация с указанным ИНН не обнаружена");
            }
            User regestration_false = new User()
            {
                Login = request.Login,
                Idpartner = partner.Id
            };
            regestration_false.Password = hasher.HashPassword(regestration_false, request.Password);
            RegestrationRequest regestration = new RegestrationRequest()
            {
                Login = regestration_false.Login,
                Partherid = (int)regestration_false.Idpartner,
                PasswordHash = regestration_false.Password,
                Description = request.message
            };
            await _context.RegestrationRequests.AddAsync(regestration);
            await _context.SaveChangesAsync();
            return new ResponseWithMessage(HttpStatusCode.Created, "");
        }
        public async Task<AuthServiceController> Auth(AuthRequest request)
        {
            User? find_user = await _context.Users.FirstOrDefaultAsync(
        i => i.Login == request.Login);

            if (find_user == null || string.IsNullOrWhiteSpace(request.Password))
            { return new AuthServiceController(HttpStatusCode.Unauthorized, "Введен неверный логин или пароль", null); }

            var result = hasher.VerifyHashedPassword(
                find_user, find_user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
            { return new AuthServiceController(HttpStatusCode.Unauthorized, "Введен неверный логин или пароль", null); }
            return new AuthServiceController(HttpStatusCode.OK, null, JWT.CreateToken(find_user));
        }
        public async Task<bool> AcceptReg(int adminID, int requestid)
        {
            try
            {
                RegestrationRequest? regestration = await _context.RegestrationRequests.FirstOrDefaultAsync(i => i.Id == requestid);
                if (regestration == null)
                {
                    return false;
                }
                regestration.ClosedAt = DateTime.Now;
                regestration.AdminId = adminID;
                regestration.IsCreated = true;
                regestration.IsClosed = true;
                if ((await _context.Users.FirstOrDefaultAsync(u => u.Login == regestration.Login)) != null)
                {
                    return false;
                }
                User createdUser = new User()
                {
                    Idpartner = regestration.Partherid,
                    Idtype = 2,
                    Login = regestration.Login,
                    Password = regestration.PasswordHash
                };
                
                await _context.Users.AddAsync(createdUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ResponseWithMessage> CreateUser(CreateUser request)
        {
            
            if (request.Login.Length < minLoginLength || request.Password.Length < minPasswordLength)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Пароль или логин не соответсвуют минимальной длине");
            }
            if (string.IsNullOrEmpty(request.Role))
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Роль пользователя не задана");
            }
            string? Email = request.Email;
            if (string.IsNullOrEmpty(request.Email))
            {
                Email = null;
            }
            string? Phone = request.Phone;
            if (string.IsNullOrEmpty(request.Phone))
            {
                Phone = null;
            }
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login);
            RegestrationRequest? regestration = await _context.RegestrationRequests.FirstOrDefaultAsync(u => u.Login == request.Login);
            if (user != null || regestration != null)
            {
                return new ResponseWithMessage(HttpStatusCode.Conflict, "Пользователь с таким логином уже существует");
            }
            if (Phone != null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone);
                if (user != null)
                {
                    return new ResponseWithMessage(HttpStatusCode.Conflict, "Телефон уже закреплен за другим пользователем");
                }
            }
            if (Email != null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user != null)
                {
                    return new ResponseWithMessage(HttpStatusCode.Conflict, "Почта уже закреплена за другим пользователем");
                }
            }
            
            UserType? Idtype = await _context.UserTypes.FirstOrDefaultAsync(i => i.Name == request.Role);
            if (Idtype == null)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Данная роль не найдена");
            }
            User created_user = new User()
            {
                Login = request.Login,
                Email = Email,
                Phone = Phone,
                Idtype = Idtype.Id
            };
            created_user.Password = hasher.HashPassword(created_user, request.Password);
            _context.Users.Add(created_user);
            await _context.SaveChangesAsync();
            return new ResponseWithMessage(HttpStatusCode.Created, "Пользователь успешно создан");
        }
        public async Task<List<string>> GetRoles()
        {
            return await _context.UserTypes.Where(r => r.Id != 2).Select(i => i.Name).ToListAsync();
        }
        public async Task<bool> DenyReg(int adminID, int requestid)
        {
            RegestrationRequest? regestration = await _context.RegestrationRequests.FirstOrDefaultAsync(i => i.Id == requestid);
            if (regestration == null)
            {
                return false;
            }
            regestration.ClosedAt = DateTime.Now;
            regestration.AdminId = adminID;
            regestration.IsCreated = false;
            regestration.IsClosed = true;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ExistingRegRequest>> GetReg()
        {
           return await _context.RegestrationRequests.Where(rr => !rr.IsClosed).Include(rr => rr.Parther).Select(i => new ExistingRegRequest(i.Id, i.Parther.Name, i.Login, i.Description, i.CreatedAt)).ToListAsync();
        }
        public async Task<ResponseWithMessage> ChangePassword(int userId, ChangePasswordRequest request)
        {
            
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            { return new ResponseWithMessage(HttpStatusCode.NotFound, "Пользователь не найден"); }

            if (string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "OldPassword и NewPassword обязательны");
            }


            var CheckNew = hasher.VerifyHashedPassword(user, user.Password, request.OldPassword);
            if (CheckNew == PasswordVerificationResult.Success)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Новый пароль не может совпадать со старым");
            }
            var verifyResult = hasher.VerifyHashedPassword(user, user.Password, request.OldPassword);
            if (verifyResult != PasswordVerificationResult.Success)
            {
                return new ResponseWithMessage(HttpStatusCode.Unauthorized, "Текущий пароль неверен");
            }

            user.Password = hasher.HashPassword(user, request.NewPassword);

            await _context.SaveChangesAsync();
            return new ResponseWithMessage(HttpStatusCode.OK, "Пароль успешно изменён");
        }
        public async Task<ResponseWithMessage> ChangePhone(int userId, ChangePhoneRequest request)
        {
            

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new ResponseWithMessage(HttpStatusCode.NotFound, "Пользователь не найден");
            }
            


            if (string.IsNullOrEmpty(request.NewPhone))
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "номер телефона не может быть пустым");
            }
            var verifyResult = hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (verifyResult != PasswordVerificationResult.Success)
            {
                return new ResponseWithMessage(HttpStatusCode.Unauthorized, "Текущий пароль неверен");
            }
            if (request.NewPhone == user.Phone)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Новый телефон должен отличаться");
            }
            if (await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.NewPhone) != null)
            {
                return new ResponseWithMessage(HttpStatusCode.Conflict, "Данный телефон уже зарегестрирован на другого пользователя");
            }
            user.Phone = request.NewPhone;
            await _context.SaveChangesAsync();
            return new ResponseWithMessage(HttpStatusCode.OK, "Контактные данные обновлены");
        }
        public async Task<ResponseWithMessage> ChangeEmail(int userId, ChangeEmailRequest request)
        {
            

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new ResponseWithMessage(HttpStatusCode.NotFound, "Пользователь не найден");
            }
            var verifyResult = hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (verifyResult != PasswordVerificationResult.Success)
            {
                return new ResponseWithMessage(HttpStatusCode.Unauthorized, "Текущий пароль неверен");
            }


            if (string.IsNullOrEmpty(request.NewEmail))
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "почта не может быть пустой");
            }
            if (request.NewEmail == user.Email)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Новая почта должна отличаться");
            }
            if (await _context.Users.FirstOrDefaultAsync(u => u.Email == request.NewEmail) != null)
            {
                return new ResponseWithMessage(HttpStatusCode.Conflict, "Данный email уже зарегестрирован на другого пользователя");
            }
            user.Email = request.NewEmail;
            await _context.SaveChangesAsync();
            return new ResponseWithMessage(HttpStatusCode.OK, "Контактные данные обновлены");
        }

    }
}
