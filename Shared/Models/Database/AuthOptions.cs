using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PerformanceProject.Shared.Models.DTO
{
    public class AuthOptions
    {
        public const string ISSUER = "https://localhost"; //server
        public const string AUDIENCE = "https://localhost:44365"; //client
        private const string KEY = "V94Aw5WPZuSzRZbuGBRXXS+xTZnF7Z8JZhI0GTjXP4BN48HQ1y6vW1ETuURkWdEy";
        public const int LIFETIME = 30;
        public const string ALGORITHM = SecurityAlgorithms.HmacSha256Signature;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
        public static SigningCredentials GetSigningCredentials()
        {
            return new SigningCredentials(GetSymmetricSecurityKey(), ALGORITHM);
        }

        public static DateTime ExpiresDate()
        {
            return DateTime.UtcNow.Add(TimeSpan.FromMinutes(LIFETIME));
        }
    }
}
