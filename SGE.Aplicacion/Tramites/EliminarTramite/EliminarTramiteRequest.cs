namespace SGE.Aplicacion.Tramites.EliminarTramite;

public record EliminarTramiteRequest
{
    Guid TramiteId;  
    Guid IdUsuario;
}