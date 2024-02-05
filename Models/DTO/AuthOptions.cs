using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PeopleManagmentSystem_API.Models.DTO
{
    public class AuthOptions
    {
        public const string ISSUER = "Server";
        public const string AUDIENCE = "Client";
        private const string KEY = "secretkey";
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
