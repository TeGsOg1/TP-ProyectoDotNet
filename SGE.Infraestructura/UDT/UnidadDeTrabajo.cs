using SGE.Aplicacion.Comun;
using SGE.Infraestructura.Datos;


namespace SGE.Infraestructura.UDT;

public sealed class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly SgeContext _context;

    public UnidadDeTrabajo(SgeContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Guardar()
    {
        // EF Core hace todo el trabajo sucio por nosotros gracias al SgeContext
        _context.SaveChanges();
    }
}
