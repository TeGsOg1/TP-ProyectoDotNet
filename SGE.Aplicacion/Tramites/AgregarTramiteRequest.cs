namespace SGE.Aplicacion.Tramites;

public record AgregarTramiteRequest(
    Guid Id,
    Guid ExpedienteId,
    string? contenido,
    string? etiqueta,
    Guid IdUsuario
);