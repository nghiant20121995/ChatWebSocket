using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebSocket.Helper
{
    public static class StringHelper
    {
        public static string GetInnerException(Exception ex)
        {
            var inner = ex.InnerException;
            if (inner == null) return ex.Message;
            while (inner != null)
            {
                var childEx = inner.InnerException;
                if (childEx == null) return inner.Message;
                inner = childEx;
            }
            return inner!.Message;
        }
    }
}
