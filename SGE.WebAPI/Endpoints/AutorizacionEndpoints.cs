using SGE.Aplicacion.CasosDeUso; 
using SGE.Aplicacion.DTOs; 

namespace SGE.WebApi.Endpoints;

public static class AutorizacionEndpoints
{
    public static void MapAutorizacionEndpoints(this IEndpointRouteBuilder app)
    {
        var authApi = app.MapGroup("/api/auth").WithTags("Autenticación");

        authApi.MapPost("/login", (LoginRequest request, LoginUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        });
        
        authApi.MapPost("/registrar", (RegistrarUsuarioRequest request, RegistrarUsuarioUseCase useCase) => 
        {
            var response = useCase.Ejecutar(request);
            return Results.Created($"/api/usuarios/{response.Id}", response);
        });
    }
}