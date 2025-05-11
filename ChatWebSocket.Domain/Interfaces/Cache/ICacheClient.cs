using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Cache
{
    public interface ICacheClient
    {
        Task<string> GetStringAsync(string key);
        string GetString(string key);
        Task SetStringAsync(string key, string value, TimeSpan? expiry = null);
    }
}
