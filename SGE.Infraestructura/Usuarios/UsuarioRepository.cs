using SGE.Aplicacion.Usuarios; // Asegurate que apunte a tu IUsuarioRepository
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Datos;

namespace SGE.Infraestructura.Expedientes.Usuarios;

public sealed class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(SgeContext context) : base(context)
    {
    }

    // Ya no necesitamos sobrescribir ObtenerPorId ni ObtenerTodos.
    // La clase base (Repository<T>) hace el trabajo, y EF Core hidrata el JSON de los permisos automáticamente.

    public Usuario? ObtenerPorCorreoElectronico(string correoElectronico)
    {
        var correoNormalizado = correoElectronico.Trim().ToLowerInvariant();
        
        // EF Core busca el usuario y ya nos lo trae con los permisos cargados
        return _dbSet.SingleOrDefault(usuario => usuario.CorreoElectronico == correoNormalizado);
    }
}
