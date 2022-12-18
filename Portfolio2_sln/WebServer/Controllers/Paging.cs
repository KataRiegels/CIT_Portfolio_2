namespace WebServer.Controllers
{
    public class Paging
    {
        public int MaxPageSize { get; set; } = 10;
        /*
         
        public Paging(int page, int pageSize, int totalItems, string method)
        {

            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var totalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize) - 1
                ;

            var firstPageUrl = totalItems > 0
                ? CreateLinkList(0, pageSize, method)
                : null;

            var prevPageUrl = page > 0 && totalItems > 0
                ? CreateLinkList(page - 1, pageSize, method)
                : null;

            var lastPageUrl = totalItems > 0
            ? CreateLinkList(totalPages, pageSize, method)
            : null;

            var currentPageUrl = CreateLinkList(page, pageSize, method);

            var nextPageUrl = page < totalPages - 1 && totalItems > 0
                ? CreateLinkList(page + 1, pageSize, method)
                : null;

            var result = new
            {
                firstPageUrl,
                prevPageUrl,
                nextPageUrl,
                lastPageUrl,
                currentPageUrl,
                totalItems,
                totalPages,
                items
            };
            return result;
        }
         */
    }
}
