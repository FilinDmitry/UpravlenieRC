using Microsoft.AspNetCore.Identity;
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
        public async Task<ResponseWithMessage> CreateArrivalOrder()
        {
            return new ResponseWithMessage(HttpStatusCode.InternalServerError, "dsa");
        }
    }
}
