using System.Security.Claims;
using SGE.Aplicacion.CasosDeUso; 
using SGE.Aplicacion.DTOs; 

namespace SGE.WebApi.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var usuariosApi = app.MapGroup("/api/usuarios").WithTags("Gestión de Usuarios");

        usuariosApi.MapGet("/", (ListarUsuariosUseCase useCase) => 
        {
            var response = useCase.Ejecutar();
            return Results.Ok(response);
        }).RequireAuthorization();

        usuariosApi.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, EliminarUsuarioUseCase useCase) => 
        {
            var idAdmin = Guid.Parse(user.FindFirst("ID")!.Value);
            useCase.Ejecutar(id, idAdmin);
            return Results.NoContent();
        }).RequireAuthorization();

        usuariosApi.MapPut("/{id:guid}/permisos", (Guid id, ModificarPermisosRequest request, ClaimsPrincipal user, ModificarPermisosUsuarioUseCase useCase) => 
        {
            var idAdmin = Guid.Parse(user.FindFirst("ID")!.Value);
            useCase.Ejecutar(id, request, idAdmin);
            return Results.NoContent();
        }).RequireAuthorization();

        usuariosApi.MapPut("/mis-datos", (ModificarMisDatosRequest request, ClaimsPrincipal user, ModificarMisDatosUseCase useCase) => 
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            useCase.Ejecutar(request, idUsuario);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}