using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SGE.WebAPI.Servicios;
using SGE.Aplicacion.Usuarios;

namespace SGE.WebAPI.Dependencias;

public static class InyeccionDeDependencias
{
    public static IServiceCollection AddAutenticacionJWT(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, ServicioDeToken>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opciones =>
            {
                opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                    )
                };
            });

        services.AddAuthorization();
        
        return services;
    }
}
