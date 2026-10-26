using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DAL;
using DTO;

namespace BLL.Services;
public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config) => _config = config;

    public AuthResponseDTO CreateToken(User user)
    {
        // המפתח הסודי מגיע ממשתנה הסביבה Jwt__Key, לא מהקוד
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // ה-claims: מה שהשרת "יודע" על המשתמש בכל בקשה
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var minutes = int.Parse(_config["Jwt:ExpiresMinutes"] ?? "2000");
        var expires = DateTime.UtcNow.AddMinutes(minutes);
        //עוד כמה זמן יפוג הטוקן

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new AuthResponseDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires
        };
    }
}