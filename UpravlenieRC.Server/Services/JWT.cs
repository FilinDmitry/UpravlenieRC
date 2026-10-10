using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Services
{
    public class JWT
    {
        public static TokenValidationParameters validation = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,

            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            NameClaimType = "name",
            RoleClaimType = "role"
        };
        public static string CreateToken(User user)
        {

            var claims = new[]
            {

                new Claim("sub", user.Id.ToString()),
                new Claim("name", user.Login),
                new Claim("role", user.IdtypeNavigation.Name)
            };

            var token = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256
                    ));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
