using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Services
{
    public class ArrivalService
    {
        private readonly RC_SkladContext _context;
        public ArrivalService(RC_SkladContext context)
        {
            _context = context;
        }
        public async Task<ResponseWithMessage> CreateArrivalOrder(int senderId, List<GoodsNeededDTOGet> goods)
        {
            if (goods.Count == 0)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Список товаров пустой");
            }
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == senderId);
                if (user?.Idpartner == null)
                {
                    return new ResponseWithMessage(HttpStatusCode.Unauthorized, "У отправителя не указан партнер, обратитесь в службу тех. поддержки");
                }
                ArrivalOrder arrivalOrder = new ArrivalOrder()
                {
                    Idpartner = (int)user.Idpartner
                };
                await _context.ArrivalOrders.AddAsync(arrivalOrder);
                List<GoodArrivalOrder> lst = new List<GoodArrivalOrder>();
                await _context.SaveChangesAsync();
                foreach (GoodsNeededDTOGet good in goods)
                {
                    lst.Add(new GoodArrivalOrder(){
                        GoodId = good.Id,
                        ArrivalOrderid = arrivalOrder.Id,
                        Amount = good.AmountToSell
                    });
                }
                await _context.GoodArrivalOrders.AddRangeAsync(lst);
                await _context.SaveChangesAsync();
                return new ResponseWithMessage(HttpStatusCode.Created, "Заявка успешно создана");
            }
            catch (Exception ex)
            {
                return new ResponseWithMessage(HttpStatusCode.InternalServerError, "Произошла непредвиденная ошибка");
            }
        }
    }
}
