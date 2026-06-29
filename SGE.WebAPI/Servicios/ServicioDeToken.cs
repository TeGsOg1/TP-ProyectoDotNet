using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SGE.Dominio.Entidades; // Ajustar a donde esté tu clase Usuario
using SGE.Aplicacion.Interfaces; 

namespace SGE.WebApi.Servicios;

public class ServicioDeToken(IConfiguration config) : ITokenProvider
{
    public string GenerarToken(Usuario usuario)
    {
        var claims = new[] {
            new Claim("ID", usuario.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}