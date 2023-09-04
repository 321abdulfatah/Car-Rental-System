using System.Net;

namespace BusinessAccessLayer.Exceptions
{
    internal class InternalServerException : CustomException
    {
        public InternalServerException(string message, List<string>? errors = default)
            : base(message, errors, HttpStatusCode.InternalServerError) { }
    }
}
