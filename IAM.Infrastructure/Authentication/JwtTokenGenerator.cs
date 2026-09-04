using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IAM.Domain.Entities;
using IAM.Domain.Services;
using Microsoft.IdentityModel.Tokens;

namespace IAM.Infrastructure.Authentication;

public class JwtTokenGenerator : ITokenGenerator
{
    // Nota: En produccion, estos valores deben venir de IConfiguration (appsetings.json)
    private const string SecretKey = "EstaEsUnaClaveSuperSecretaParaFirmarElTokenJWTQueDebeSerLarga";
    private const string Issuer = "IAMSystem";
    private const string Audience = "IAMSystemUsers";

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = Issuer,
            Audience = Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}