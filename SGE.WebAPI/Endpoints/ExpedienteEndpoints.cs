using System.Security.Claims;
using SGE.Aplicacion.CasosDeUso; 
using SGE.Aplicacion.DTOs; 

namespace SGE.WebApi.Endpoints;

public static class ExpedienteEndpoints
{
    public static void MapExpedienteEndpoints(this IEndpointRouteBuilder app)
    {
        var expedientesApi = app.MapGroup("/api/expedientes").WithTags("Gestión de Expedientes");

        expedientesApi.MapGet("/", (ListarExpedientesUseCase useCase) => 
        {
            var response = useCase.Ejecutar();
            return Results.Ok(response);
        }).RequireAuthorization();

        expedientesApi.MapPost("/", (AltaExpedienteRequest request, ClaimsPrincipal user, AltaExpedienteUseCase useCase) =>
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Created($"/api/expedientes/{response.Id}", response);
        }).RequireAuthorization();

        expedientesApi.MapPut("/{id:guid}/caratula", (Guid id, ModificarCaratulaExpedienteRequest request, ClaimsPrincipal user, ActualizarCaratulaExpedienteUseCase useCase) =>
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            useCase.Ejecutar(request, idUsuario);
            return Results.NoContent();
        }).RequireAuthorization();

        expedientesApi.MapPut("/{id:guid}/estado", (Guid id, ModificarEstadoExpedienteRequest request, ClaimsPrincipal user, ActualizarEstadoExpedienteUseCase useCase) =>
        {
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            useCase.Ejecutar(request, idUsuario);
            return Results.NoContent();
        }).RequireAuthorization();

        expedientesApi.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, EliminarExpedienteUseCase useCase) =>
        {
            var request = new EliminarExpedienteRequest { Id = id };
            var idUsuario = Guid.Parse(user.FindFirst("ID")!.Value);
            useCase.Ejecutar(request, idUsuario);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}