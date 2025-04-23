using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Interfaces
{
    public interface IModifiedDate
    {
        DateTime? ModifiedDate { get; set; }
    }
}
