using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatWebSocketHelper
{
    public static class JwtHandler
    {
        public static string GenerateToken(string email, string secretKey, DateTime expAt, string issuer, string audience)
        {
            var claims = new[]
            {
                new Claim("Email", email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var rsa = RSA.Create();
            rsa.ImportFromPem(secretKey);

            var key = new RsaSecurityKey(rsa);
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expAt,
                signingCredentials: creds);

            var newToken = new JwtSecurityTokenHandler().WriteToken(token);
            return newToken;
        }

        public static bool ValidateToken(string token, string publicKey, string issuer, string audience)
        {
            // 1. Load your public key
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);

            var key = new RsaSecurityKey(rsa);

            // 2. Configure validation parameters
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateLifetime = true, // optional: verify expiration
                RequireExpirationTime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                ClockSkew = TimeSpan.FromMinutes(1) // optional: token lifetime leeway
            };

            // 3. Validate the token
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = handler.ValidateToken(token, validationParams, out SecurityToken validatedToken);
            return principal != null && validatedToken != null;
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
    }
}
