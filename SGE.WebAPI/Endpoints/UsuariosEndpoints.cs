using System.Security.Claims;
using SGE.Aplicacion.Usuarios;

namespace SGE.WebAPI.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var usuariosApi = app.MapGroup("/api/usuarios").WithTags("Gestión de Usuarios");

        usuariosApi.MapGet("/", (ClaimsPrincipal user, ListarUsuariosUseCase useCase) => 
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var request = new ListarUsuariosRequest(idUsuario);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).RequireAuthorization();

        usuariosApi.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, EliminarUsuarioUseCase useCase) => 
        {
            var idAdmin = Guid.Parse(user.FindFirst("ID")!.Value);
            var request = new EliminarUsuarioRequest(id, idAdmin);
            useCase.Ejecutar(request);
            return Results.NoContent();
        }).RequireAuthorization();

        usuariosApi.MapPut("/{id:guid}/permisos", (Guid id, ModificarPermisosUsuarioRequest request, ClaimsPrincipal user, ModificarPermisosUsuarioUseCase useCase) => 
        {
            var idAdmin = Guid.Parse(user.FindFirst("ID")!.Value);
            var requestConIds = request with { IdUsuarioObjetivo = id, IdUsuarioEjecutor = idAdmin };
            useCase.Ejecutar(requestConIds);
            return Results.NoContent();
        }).RequireAuthorization();

        usuariosApi.MapPut("/mis-datos", (ModificarMisDatosRequest request, ClaimsPrincipal user, ModificarMisDatosUseCase useCase) => 
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var requestConIds = request with { IdUsuarioToken = idUsuario, IdUsuarioObjetivo = idUsuario };
            useCase.Ejecutar(requestConIds);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}