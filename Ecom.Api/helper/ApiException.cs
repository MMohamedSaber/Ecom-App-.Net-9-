namespace Ecom.Api.helper
{
    public class ApiException : ResponsApi
    {
        private readonly string _details;

        public ApiException(int statusCode, string message = null,string details=null) : base(statusCode, message)
        {
            _details = details;
        }
    }
}
