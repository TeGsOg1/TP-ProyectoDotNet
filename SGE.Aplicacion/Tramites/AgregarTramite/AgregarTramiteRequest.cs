namespace SGE.Aplicacion.Tramites.AgregarTramite;

public record AgregarTramiteRequest(
    Guid Id,
    Guid ExpedienteId,
    string? Contenido,
    string? Etiqueta,
    Guid IdUsuario
);