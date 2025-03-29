using StockSense.Data;
using System.Security.Claims;

namespace StockSense.Services
{
    public class BaseService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            return int.TryParse(userId, out var Id) ? Id : 0;
        }
    }
}
