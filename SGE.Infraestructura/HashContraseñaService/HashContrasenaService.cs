using System.Security.Cryptography;
using System.Text;
using SGE.Aplicacion.Usuarios;

namespace SGE.Infraestructura.HashContrasenas;

public class HashContrasenaService : IHashContrasenaService
{
    public string Hashear(string contrasenaEnTextoPlano)
    {
        var bytes = Encoding.UTF8.GetBytes(contrasenaEnTextoPlano);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
