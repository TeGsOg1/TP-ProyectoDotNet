namespace SGE.Aplicacion.Tramites.AgregarTramite;

public record AgregarTramiteRequest(
    Guid Id,
    Guid ExpedienteId,
    string? contenido,
    string? etiqueta,
    Guid IdUsuario
);