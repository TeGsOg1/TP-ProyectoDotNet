namespace SGE.Aplicacion.Usuarios;

public interface IHashContrasenaService
{
    string Hashear(string contrasenaEnTextoPlano);
}
