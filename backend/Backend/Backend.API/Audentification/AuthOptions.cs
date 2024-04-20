using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Backend.API.Audentification;

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    private const string KEY = "mysupersecret_secretkey123123!123!!";   // ключ для шифрации
    public const int LIFETIME = 450; // время жизни токена - 1 минута
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}