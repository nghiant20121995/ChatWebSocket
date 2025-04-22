using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Exceptions
{
    public class ValidateException : Exception
    {
        public ValidateException(string msg) : base(msg) { }
    }
}
