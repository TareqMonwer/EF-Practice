using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Drawing.Printing;
using System.Linq;

namespace WebApplication2.Schema
{
    public class AppConfigs
    {
        public int PageSize { get; set; }
    }

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        private static AppConfigs? GetAppConfigs()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var appConfigsSection = config.GetSection("AppConfigs");
            return appConfigsSection.Get<AppConfigs>();
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex)
        {
            var appConfigs = GetAppConfigs();
            var defaultPageSize = appConfigs?.PageSize ?? 10;

            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * defaultPageSize).Take(defaultPageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, defaultPageSize);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var appConfigs = GetAppConfigs();
            var defaultPageSize = appConfigs?.PageSize ?? pageSize;

            return new PaginatedList<T>(items, count, pageIndex, defaultPageSize);
        }
    }
}
