namespace SGE.Aplicacion.Tramites;
public interface ITramiteRepository
{
    void AgregarTramite (Tramite tramite);
    Tramite? ObtenerTramiteId (Guid TramiteId);
    IEnumerable<Tramite> ObtenerTodosTramite();
    void ModificarTramite(Tramite t);
    void EliminarTramite(Guid TramiteId);
    
}