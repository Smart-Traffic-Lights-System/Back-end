using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UserStore;

public static class Setup
{
    public static string token;
    public static IConfiguration configRoot;

    public static void AdminLogin()
    {
        var userId = 1;
        var role = "Administrator";
        var username = "athadmin";
        var tokenJwt = GetToken(userId, username, role);
        var tokenHandler = new JwtSecurityTokenHandler();
        token = tokenHandler.WriteToken(tokenJwt);
    }

    private static JwtSecurityToken GetToken(int userId, string username, string role) // Add userId, username and role parameters
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configRoot["JWT:Key"]));

        var token = new JwtSecurityToken(
            issuer: configRoot["JWT:Issuer"],
            audience: configRoot["JWT:Audience"],
            expires: DateTime.Now.AddDays(1),
            claims: new[]
            {
                new Claim("userId", userId.ToString()), // Add userId claim
                new Claim("username", username),        // Add username claim
                new Claim("role", role)                 // Add role claim
            },
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

}
