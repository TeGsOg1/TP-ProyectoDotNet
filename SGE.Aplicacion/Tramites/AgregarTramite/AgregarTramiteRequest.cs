namespace SGE.Aplicacion.Tramites.AgregarTramite;

public record AgregarTramiteRequest(
    Guid Id,
    Guid ExpedienteId,
    ContenidoTramite Contenido,
    EtiquetaTramite Etiqueta,
    Guid IdUsuario
);