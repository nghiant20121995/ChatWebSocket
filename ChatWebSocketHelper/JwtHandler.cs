using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatWebSocketHelper
{
    public static class JwtHandler
    {
        public static string GenerateToken(string username, string secretKey, DateTime expAt, string issuer, string audience)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            using var rsa = RSA.Create();
            rsa.ImportFromPem(secretKey);

            var key = new RsaSecurityKey(rsa);
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expAt,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string ExportPrivateKeyPem(RSA rsa)
        {
            var privateKeyBytes = rsa.ExportPkcs8PrivateKey();
            var base64 = Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks);
            return $"-----BEGIN PRIVATE KEY-----\n{base64}\n-----END PRIVATE KEY-----";
        }

        public static string ExportPublicKeyPem(RSA rsa)
        {
            var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            var base64 = Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks);
            return $"-----BEGIN PRIVATE KEY-----\n{base64}\n-----END PRIVATE KEY-----";
        }

        public static void LoadSecretKey(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var jwtSection = configuration.GetSection("Jwt");
            if (jwtSection != null)
            {
                var privateKey = File.ReadAllText(jwtSection?.GetSection("PrivateKeyPath")?.Value ?? string.Empty);
                if (!string.IsNullOrEmpty(privateKey))
                {
                    configuration["Jwt:PrivateKey"] = privateKey;
                }
                var publicKey = File.ReadAllText(jwtSection?.GetSection("PublicKeyPath")?.Value ?? string.Empty);
                if (!string.IsNullOrEmpty(publicKey))
                {
                    configuration["Jwt:PublicKey"] = publicKey;
                }
            }
        }
    }
}
