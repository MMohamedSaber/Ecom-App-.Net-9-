
namespace Ecom.Core.Sharing
{
    public class ProductParams
    {

        public string? Sort { get; set; }
        public int? CategoryId { get; set; }
        public string? Searching { get; set; }

        public int MaxPageSize { get; set; } = 6;

        private int _pageSize = 3;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageNumber { get; set; } = 1;

    }
}
