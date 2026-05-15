namespace SGE.Aplicacion.Tramites;
public interface ITramiteRepository
{
    void AgregarTramite (Tramite tramite);
    Tramite? ObtenerTramiteId (Guid id);
    IEnumerable<Tramite> ObtenerPorTramiteId(Guid expedienteId);
    void ModificarTramite(Tramite t);
    void EliminarTramite(Guid Id);
    void EliminarPorTramiteId (Guid expedienteId);
}