using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UserManagement;

public static class Setup
{
    public static string token;
    public static IConfiguration configRoot;

    public static void AdminLogin()
    {
        var userId = 1;
        var role = 1;
        var username = "athanasiou";
        var tokenJwt = GenerateToken(userId.ToString(), username, role, configRoot["JWT:Key"], configRoot["JWT:Issuer"], configRoot["JWT:Audience"]);
    }

    private static JwtSecurityToken GenerateToken(string userId, string username, int roleId, string key, string issuer, string audience)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.Now.AddHours(1),
            claims: new[]
            {
                new Claim("UserId", userId.ToString()),
                new Claim("Username", username),
                new Claim("RoleId", roleId.ToString())
            },
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

}
