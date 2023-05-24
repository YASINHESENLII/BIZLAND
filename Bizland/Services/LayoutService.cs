using BIZLAND.DAL;
using Microsoft.EntityFrameworkCore;

namespace BIZLAND.Service
{
    public class LayoutService
    {
        readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetSettings()
        {
            var settings = await _context.settings.ToDictionaryAsync(setting => setting.Key,setting=>setting.Value);

            return settings;
        }



    }
}
