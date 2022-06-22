using System;

namespace TicketsManager.Models.AppExceptions
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {

        }
    }
}
