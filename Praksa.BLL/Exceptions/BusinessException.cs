using System.Net;

namespace Praksa.BLL.Exceptions
{
    public class BusinessException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }


        public BusinessException(string Message, HttpStatusCode statusCode) : base(Message)
        {
            this.StatusCode = statusCode;
        }
    }
}
