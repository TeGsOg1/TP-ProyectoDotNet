namespace SGE.Aplicacion.Tramites;
    public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; }
    public ContenidoTramite Contenido { get; private set; }
    public EtiquetaTramite Etiqueta { get; private set; }
    public DateTime FechaCreacion { get; private set;}
    public DateTime FechaUltimaModificacion {get; private set;}
    public Guid IdUsuarioUltimoCambio { get; private set; }
}