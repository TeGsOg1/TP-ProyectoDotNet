
namespace SGE.Aplicacion.Tramites.ModificarTramite;

public record ModificarTramiteRequest(
    Guid IdUsuario;
    ContenidoTramite Contenido,
    EtiquetaTramite Etiqueta,
);