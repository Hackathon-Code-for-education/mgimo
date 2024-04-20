using System.Security.Claims;
using Backend.Domain.Entity;

namespace Backend.Core.Audentification;

public class ApplicantAudentificationService
{
    public  ClaimsIdentity GetIdentity(string login, string role, int id)
    { 
        var claims = new List<Claim> 
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, login),
            new Claim(ClaimTypes.Sid, id.ToString()),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            
        };
        
        var claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}