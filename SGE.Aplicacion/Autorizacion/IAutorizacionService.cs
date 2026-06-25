using SGE.Dominio.Autorizacion;

namespace SGE.Aplicacion.Autorizacion;

public interface IAutorizacionService {

    bool PoseeElPermiso (Guid IdUsuario, Permiso permiso);

}
