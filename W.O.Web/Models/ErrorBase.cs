using FluentResults;

namespace W.O.Web.Models
{
    public class ErrorBase : Error
    {
        protected ErrorBase(int statusCode)
        {
            StatusCode = statusCode;
        }
        public int StatusCode { get; }
    }
}
