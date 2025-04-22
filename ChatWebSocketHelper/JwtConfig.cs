using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Helper
{
    public class JwtConfig
    {
        public string PrivateKeyPath { get; set; }
        public string PublicKeyPath { get; set; }
        public string Audience {  get; set; }
        public string Issuer { get; set; }
        public double DayDuration { get; set; }
    }
}
