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
        public async Task<GoodsDTO> GetGood(int id)
        {
            Good? good = await _context.Goods.FirstOrDefaultAsync(i => i.Id == id);
            if (good == null)
            {
                return null;
            }
            else
            {
                return GoodsDTO.CreateGoodsDTO(good);
            }
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
        public async Task<ResponseWithMessage> ChangeGood(int goodId, GoodsDTO good)
        {
            try
            {
                Good? goodExisted = await _context.Goods.FirstOrDefaultAsync(i => i.Id == good.Id);
                if (goodId == 0 || goodExisted == null)
                {
                    if (await _context.TovarCategories.FirstOrDefaultAsync(i => i.Id == good.CategoryID) == null)
                    {
                        return new ResponseWithMessage(HttpStatusCode.BadRequest, "Такой категории не найдено");
                    }
                    if (await _context.Units.FirstOrDefaultAsync(i => i.Id == good.UnitsID) == null)
                    {
                        return new ResponseWithMessage(HttpStatusCode.BadRequest, "Такой единицы измерения не найдено");
                    }
                    if (await _context.Manufacturers.FirstOrDefaultAsync(i => i.Id == good.ManufacturerID) == null)
                    {
                        return new ResponseWithMessage(HttpStatusCode.BadRequest, "Такой производитель не найден");
                    }
                    Good createdGood = GoodsDTO.CreateGoodFromDTO(good);
                    
                    await _context.Goods.AddAsync(createdGood);
                    await _context.SaveChangesAsync();
                    GoodsNeeded needed = new GoodsNeeded()
                    {
                        GoodId = createdGood.Id,
                        Amount = 0,
                        Price = good.Price,
                    };
                    await _context.GoodsNeededs.AddAsync(needed);
                    await _context.SaveChangesAsync();
                    return new ResponseWithMessage(HttpStatusCode.Created, "Новый товар успешно создан");
                }
                else
                {
                    
                    
                    if (await _context.TovarCategories.FirstOrDefaultAsync(i => i.Id == good.CategoryID) == null)
                    {
                        return new ResponseWithMessage(HttpStatusCode.BadRequest, "Такой категории не найдено");
                    }
                    if (await _context.Units.FirstOrDefaultAsync(i => i.Id == good.UnitsID) == null)
                    {
                        return new ResponseWithMessage(HttpStatusCode.BadRequest, "Такой единицы измерения не найдено");
                    }
                    if (await _context.Manufacturers.FirstOrDefaultAsync(i => i.Id == good.ManufacturerID) == null)
                    {
                        return new ResponseWithMessage(HttpStatusCode.BadRequest, "Такой производитель не найден");
                    }
                    goodExisted.Name = good.Name;
                    goodExisted.Mintemperature = good.Mintemperature;
                    goodExisted.Maxtemperature = good.Maxtemperature;
                    goodExisted.Idmanufacturer = good.ManufacturerID;
                    goodExisted.Idunits = good.UnitsID;
                    goodExisted.IdCategory = good.CategoryID;
                    goodExisted.IsInStopList = false;
                    goodExisted.VolumeInLitr = good.VolumeInLitr;
                    goodExisted.WeightForOneUnit = good.WeightForOneUnit;
                    if (goodExisted.GoodsNeeded != null)
                    {
                        goodExisted.GoodsNeeded.Price = good.Price;
                    }
                    await _context.SaveChangesAsync();
                    return new ResponseWithMessage(HttpStatusCode.OK, "Товар успешно изменен");
                }
            }
            catch (Exception ex)
            {
                return new ResponseWithMessage(HttpStatusCode.InternalServerError, "произошла непредвиденная ошибка");
            }
            
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
        public async Task<List<NameWithIdDTO>> GetCategories()
        {
            return await _context.TovarCategories.Select(c => new NameWithIdDTO(c.Id, c.Name)).ToListAsync();
        }
        public async Task<List<NameWithIdDTO>> GetManufacturers()
        {
            return await _context.Manufacturers.Select(c => new NameWithIdDTO(c.Id, c.Name)).ToListAsync();
        }
        public async Task<List<NameWithIdDTO>> GetUnits()
        {
            return await _context.Units.Select(c => new NameWithIdDTO(c.Id, c.Name)).ToListAsync();
        }
    }
}
