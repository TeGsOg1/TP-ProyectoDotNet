using System.Security.Claims;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;

namespace SGE.WebAPI.Endpoints;

public static class ExpedienteEndpoints
{
    public static void MapExpedienteEndpoints(this IEndpointRouteBuilder app)
    {
        var expedientesApi = app.MapGroup("/api/expedientes").WithTags("Gestión de Expedientes");

        expedientesApi.MapGet("/", (ObtenerTodosExpedientesUseCase useCase) => 
        {
            var response = useCase.Ejecutar();
            return Results.Ok(response);
        }).RequireAuthorization();

        expedientesApi.MapPost("/", (AltaExpedienteRequest request, ClaimsPrincipal user, AltaExpedienteUseCase useCase) =>
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var requestConIdUsuario = request with { IdUsuario = idUsuario };
            var response = useCase.Ejecutar(requestConIdUsuario);
            return Results.Created($"/api/expedientes/{response.IdExpediente}", response);
        }).RequireAuthorization();

        expedientesApi.MapPut("/{id:guid}/caratula", (Guid id, ModificarCaratulaExpedienteRequest request, ClaimsPrincipal user, ActualizarCaratulaExpedienteUseCase useCase) =>
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var requestConIdUsuario = request with { IdUsuario = idUsuario };
            useCase.Ejecutar(requestConIdUsuario);
            return Results.NoContent();
        }).RequireAuthorization();

        expedientesApi.MapPut("/{id:guid}/estado", (Guid id, ModificarEstadoExpedienteRequest request, ClaimsPrincipal user, ActualizarEstadoExpedienteUseCase useCase) =>
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var requestConIdUsuario = request with { IdUsuario = idUsuario };
            useCase.Ejecutar(requestConIdUsuario);
            return Results.NoContent();
        }).RequireAuthorization();

        expedientesApi.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, EliminarExpedienteUseCase useCase) =>
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var request = new EliminarExpedienteRequest(id, idUsuario);
            useCase.Ejecutar(request);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}