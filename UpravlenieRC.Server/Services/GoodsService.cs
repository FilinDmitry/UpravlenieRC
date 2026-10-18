using Microsoft.EntityFrameworkCore;
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
            return await _context.Goods
                .Include(g => g.IdCategoryNavigation)
                .Include(g => g.IdunitsNavigation)
                .Include(g => g.IdmanufacturerNavigation)
                .Select(u => new GoodsDTO(u)).ToListAsync();
        }
    }
}
