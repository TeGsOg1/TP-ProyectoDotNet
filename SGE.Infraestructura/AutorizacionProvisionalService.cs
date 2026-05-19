using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura.Autorizacion;
public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid IdUsuario, Permiso permiso) {
        return true;
    }
}