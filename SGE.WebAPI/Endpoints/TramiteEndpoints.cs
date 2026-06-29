using System.Security.Claims;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Usuarios;

namespace SGE.WebAPI.Endpoints;

public static class TramiteEndpoints
{
    public static void MapTramiteEndpoints(this IEndpointRouteBuilder app)
    {
        var tramitesApi = app.MapGroup("/api/tramites").WithTags("Gestión de Trámites");

        tramitesApi.MapPost("/", (AgregarTramiteRequest request, ClaimsPrincipal user, AgregarTramiteUseCase useCase) => 
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var requestConIdUsuario = request with { IdUsuario = idUsuario };
            var response = useCase.Ejecutar(requestConIdUsuario);
            return Results.Created($"/api/tramites/{response.Id}", response);
        }).RequireAuthorization();

        tramitesApi.MapGet("/{expedienteId:guid}", (Guid expedienteId, ListarTramitesUseCase useCase) => 
        {
            var request = new ListarTramiteRequest(expedienteId);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).RequireAuthorization();

        tramitesApi.MapPut("/{id:guid}", (Guid id, ModificarTramiteRequest request, ClaimsPrincipal user, ModificarTramiteUseCase useCase) => 
        {
            if (id != request.Id) return Results.BadRequest(); 
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var requestConIdUsuario = request with { IdUsuario = idUsuario };
            useCase.Ejecutar(requestConIdUsuario);
            return Results.NoContent();
        }).RequireAuthorization();

        tramitesApi.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, EliminarTramiteUseCase useCase) => 
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var request = new EliminarTramiteRequest(id, idUsuario);
            useCase.Ejecutar(request);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}