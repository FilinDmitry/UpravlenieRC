using Microsoft.EntityFrameworkCore;
using System.Net;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Services
{
    public class GoodsService
    {
        private readonly RC_SkladContext _context;
        public GoodsService(RC_SkladContext context)
        {
            _context = context;
        }
        public async Task<List<GoodsDTO>> GetGoods()
        {
            return await _context.Goods.Where(g => !g.IsInStopList)
                .Include(g => g.IdCategoryNavigation)
                .Include(g => g.IdunitsNavigation)
                .Include(g => g.IdmanufacturerNavigation)
                .Select(u => GoodsDTO.CreateGoodsDTO(u)).ToListAsync();
        }
        public async Task<List<GoodsNeededInfo>> GetNeededGoods()
        {
            return await _context.GoodsNeededs
                .Include(gn => gn.Good).Where(g => !g.Good.IsInStopList)
                .Include(gn => gn.Good.IdmanufacturerNavigation)
                .Include(gn => gn.Good.IdCategoryNavigation)
                .Include(gn => gn.Good.IdunitsNavigation).Select(gn => new GoodsNeededInfo(gn)).ToListAsync();
        }
        public async Task<ResponseWithMessage> StopGood(int goodId)
        {
            Good? good_to_del = await _context.Goods.FirstOrDefaultAsync(g => g.Id == goodId);
            if (good_to_del == null)
            {
                return new ResponseWithMessage(HttpStatusCode.NotFound, "Товар с таким id не найден");
            }
            good_to_del.IsInStopList = true;
            if (good_to_del.GoodsNeeded != null)
            {
                good_to_del.GoodsNeeded.Amount = 0;
            }
            await _context.SaveChangesAsync();
            return new ResponseWithMessage(HttpStatusCode.NoContent, "");
        }
        public async Task<ResponseWithMessage> ChangeGood(int goodId, GoodsDTO goods)
        {
            
            return new ResponseWithMessage(HttpStatusCode.NotImplemented, "");
        }
        public async Task<ResponseWithMessage> SetNeededGoods(int id, int amount, double? price)
        {
            GoodsNeeded? needed = await _context.GoodsNeededs.FirstOrDefaultAsync(i => i.GoodId == id);
            if (amount <= 0)
            {
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Количество должно быть больше 0");
            }
            if (needed == null)
            {
                if (await _context.Goods.FirstOrDefaultAsync(i => i.Id == id) == null)
                {
                    return new ResponseWithMessage(HttpStatusCode.BadRequest, "Товара с таким id не существует");
                }
                if (price != null)
                {
                    if (price > 0)
                    {
                        await _context.GoodsNeededs.AddAsync(new GoodsNeeded { GoodId = id, Amount = amount, Price = (double)price });
                        await _context.SaveChangesAsync();
                        return new ResponseWithMessage(HttpStatusCode.OK, "Товар успешно записан");
                    }
                }
                
                return new ResponseWithMessage(HttpStatusCode.BadRequest, "Цена должна быть => 0");
                
            }
            
            needed.Amount = amount;
            if (price != null)
            {
                needed.Price = (double)price;
                await _context.SaveChangesAsync();
                return new ResponseWithMessage(HttpStatusCode.OK, "Цена и количество успешно изменены");
            }
            await _context.SaveChangesAsync();
            return new ResponseWithMessage(HttpStatusCode.OK, "Количество успешно изменено");
        }
        public async Task<List<string>> GetCategories()
        {
            return await _context.TovarCategories.Select(c => c.Name).ToListAsync();
        }
    }
}
