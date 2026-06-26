using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Enums;

namespace SGE.Infraestructura.Autorizacion;
public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid IdUsuario, Permiso permiso) {
        return true;
    }
}
