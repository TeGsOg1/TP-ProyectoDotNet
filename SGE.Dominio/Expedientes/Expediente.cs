public class Expediente
{
    public Guid Id { get; private set; }
    public object Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public DateTime fechaUltimoCambio { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public enum Estado
    {
        RecienIniciado,
        ParaResolver,
        ConResolucion,
        EnNotificacion,
        Finalizado
    }

    public Expediente(Guid id, object caratula, DateTime fechaCreacion, DateTime fechaUltimaModificacion, DateTime fechaUltimoCambio, Guid usuarioUltimoCambio)
    {
        Id = id;
        Caratula = caratula;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaUltimaModificacion;
        this.fechaUltimoCambio = fechaUltimoCambio;
        UsuarioUltimoCambio = usuarioUltimoCambio;
        Estado = Estado.RecienIniciado;
    }
    


}