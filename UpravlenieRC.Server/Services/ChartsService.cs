using Microsoft.EntityFrameworkCore;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Services
{
    public class ChartsService
    {
        private readonly RC_SkladContext _context;
        public ChartsService(RC_SkladContext context)
        {
            _context = context;
        }
        public async Task<List<OrdersByDateDto>> GetOrdersArrivalData()
        {
            var now = DateTime.UtcNow;
            var startDate = now.Date.AddDays(-9);

            var rawData = await _context.ArrivalOrders
    .Where(o => o.CreatedAt >= startDate)
    .GroupBy(o => o.CreatedAt.Date)
    .Select(g => new
    {
        Date = g.Key,
        Count = g.Count()
    })
    .ToListAsync();
            var data = rawData
    .Select(x => new OrdersByDateDto(x.Date, x.Count)
    )
    .OrderBy(x => x.Date)
    .ToList();
            var dict = data.ToDictionary(x => x.Date, x => x.Count);

            var fullData = Enumerable.Range(0, 10)
                .Select(i =>
                {
                    var d = startDate.AddDays(i).Date;
                    return new OrdersByDateDto(d, dict.TryGetValue(d, out var c) ? c : 0);
                })
                .ToList();
            return fullData;
        }

    }
}
