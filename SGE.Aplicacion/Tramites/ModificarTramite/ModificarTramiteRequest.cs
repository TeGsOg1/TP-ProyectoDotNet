namespace SGE.Aplicacion.Tramites.ModificarTramite;

public record ModificarTramiteRequest(
    Guid Id,
    ContenidoTramite Contenido,
    EtiquetaTramite Etiqueta,
    Guid IdUsuario
);