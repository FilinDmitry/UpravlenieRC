using Microsoft.EntityFrameworkCore;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Services
{
    public class CellBalanceServices
    {
        private readonly RC_SkladContext _context;
        public CellBalanceServices(RC_SkladContext context)
        {
            _context = context;
        }
        public async Task<List<CellBalanseDTO>> GetCellBalanse()
        { // TODO оптимизировать это недоразумение данный фрагмент
            return 
                await _context.CellBalances
                .AsNoTracking()
                .Include(cb => cb.IdcellNavigation)
                .Include(cb => cb.IdtovarNavigation)
                    .ThenInclude(t => t.IdgoodNavigation)
                        .ThenInclude(g => g.IdCategoryNavigation)      
                .Include(cb => cb.IdtovarNavigation)             
                    .ThenInclude(t => t.IdgoodNavigation)
                        .ThenInclude(g => g.IdmanufacturerNavigation)
                .Include(cb => cb.IdtovarNavigation) 
                    .ThenInclude(t => t.IdgoodNavigation)
                        .ThenInclude(g => g.IdunitsNavigation)
                .Select(cb => new CellBalanseDTO(cb))
                .ToListAsync();
        }
    }
}
