using E_Commerce.Application.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services;

public class TokenService : ITokenService
{
    public async Task<string> CreateTokenAsync(string userId, string email, string userName, IReadOnlyList<string> roles)
    {
        // Header [type, algo]

        // Payloads [claims]

        // Signature (SecretKey)

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.GivenName, userName),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }


        var secretKey = "MYSECRETKEYforMyApplicationMYSECRETKEYforMyApplicationMYSECRETKEYforMyApplicationMYSECRETKEYforMyApplication";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var jwtToken = new JwtSecurityToken(
            issuer: "https://localhost:7116",
            audience: "MyOnlineStore",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(2),
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}
