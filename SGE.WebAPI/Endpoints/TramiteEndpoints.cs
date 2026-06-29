using System.Security.Claims;
using SGE.Aplicacion.CasosDeUso; 
using SGE.Aplicacion.DTOs; 

namespace SGE.WebApi.Endpoints;

public static class TramiteEndpoints
{
    public static void MapTramiteEndpoints(this IEndpointRouteBuilder app)
    {
        var tramitesApi = app.MapGroup("/api/tramites").WithTags("Gestión de Trámites");

        tramitesApi.MapPost("/", (AgregarTramiteRequest request, ClaimsPrincipal user, AgregarTramiteUseCase useCase) => 
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Created($"/api/tramites/{response.Id}", response);
        }).RequireAuthorization();

        tramitesApi.MapGet("/", (ListarTramitesUseCase useCase) => 
        {
            var response = useCase.Ejecutar();
            return Results.Ok(response);
        }).RequireAuthorization();

        tramitesApi.MapPut("/{id:guid}", (Guid id, ModificarTramiteRequest request, ClaimsPrincipal user, ModificarTramiteUseCase useCase) => 
        {
            if (id != request.Id) return Results.BadRequest(); 
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            useCase.Ejecutar(request, idUsuario);
            return Results.NoContent();
        }).RequireAuthorization();

        tramitesApi.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, EliminarTramiteUseCase useCase) => 
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            useCase.Ejecutar(id, idUsuario);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}