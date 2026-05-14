namespace SGE.Aplicacion.Tramites;
public interface ITramiteRepository
{
    void AgregarTramite (Tramite tramite);
    Tramites? ObtenerTramiteId (Guid id);
    //IEnumerable<Tramite> ObtenerPorExpeienteId(Guid expedienteId);
    void ModificarTramite(Tramites t);
    void EliminarTramite(Guid Id);
    void EliminarPorExpedienteI (Guid expedienteId);
}