using System;

namespace DesafioMbLabs.Models.AppExceptions
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {

        }
    }
}
