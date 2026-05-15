namespace SGE.Aplicacion.Tramites.AgregarTramite;

public record AgregarTramiteRequest
{
    Guid TramiteId
    ContenidoTramite Contenido;
    EtiquetaTramite Etiqueta;
    Guid IdUsuario;

}