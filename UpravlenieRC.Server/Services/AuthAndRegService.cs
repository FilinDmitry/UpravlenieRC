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
        private readonly PasswordHasher<RegestrationRequest> regHasher = new PasswordHasher<RegestrationRequest>();
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
            RegestrationRequest regestration = new RegestrationRequest()
            {
                Description = request.message,
                Login = request.Login,
                PasswordHash = request.Password,
                Partherid = partner.Id
            };
            regestration.PasswordHash = regHasher.HashPassword(regestration, request.Password);
            _context.RegestrationRequests.Add(regestration);
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
