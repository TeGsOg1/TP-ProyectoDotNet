public class Expediente
{
    public Guid Id { get; private set; }
    public object Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public EstadoEnum Estado { get; private set; }

    public Expediente(Guid id, object caratula, Guid usuarioUltimoCambio)
    {
        Id = id;
        Caratula = caratula;
        FechaCreacion = DateTime.Now;
        FechaUltimaModificacion = DateTime.Now;
        UsuarioUltimoCambio = usuarioUltimoCambio;
        Estado = Estado.RecienIniciado;
    }
    
    public void ActualizarEstado (Estado nuevoEstado, Guid usuarioCambio)
    {
        Estado = nuevoEstado;
        if (FechaUltimaModificacion > FechaCreacion)
        {
            FechaUltimaModificacion = DateTime.Now;
        }
        UsuarioUltimoCambio = usuarioCambio;
    }

}