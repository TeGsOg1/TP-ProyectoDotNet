using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SGE.WebApi.Servicios;
using SGE.Aplicacion.Usuarios; // Asumiendo que ITokenProvider está acá

namespace SGE.WebApi.Dependencias;

public static class InyeccionDeDependencias
{
    public static IServiceCollection AddAutenticacionJWT(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenProvider, ServicioDeToken>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opciones =>
            {
                opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

        services.AddAuthorization();
        return services;
    }
}