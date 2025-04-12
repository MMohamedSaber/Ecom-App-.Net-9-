namespace Ecom.Api.helper
{
    public class Pagenation<T> where T : class
    {
        public Pagenation(int pageSize, int pageNumber, int totalCount, IEnumerable<T> data)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalCount = totalCount;
            Data = data;
        }

        public int PageSize { get; set; }
        public int PageNumber {  get; set; }

        public int TotalCount{ get; set; }
        public IEnumerable<T> Data{ get; set; }

    }
}
