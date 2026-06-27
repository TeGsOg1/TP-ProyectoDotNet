using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Usuarios;

public class LoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashContrasenaService _hashContrasenaService;
    private readonly ITokenService _tokenService;

    public LoginUseCase(
        IUsuarioRepository usuarioRepository,
        IHashContrasenaService hashContrasenaService,
        ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _hashContrasenaService = hashContrasenaService;
        _tokenService = tokenService;
    }

    public LoginResponse Ejecutar(LoginRequest request)
    {
        var usuario = _usuarioRepository.ObtenerPorCorreoElectronico(request.CorreoElectronico);
        if (usuario is null)
        {
            throw new AutorizacionException("Credenciales inválidas.");
        }

        var contrasenaHashIngresada = _hashContrasenaService.Hashear(request.Contrasena);
        if (usuario.ContrasenaHash != contrasenaHashIngresada)
        {
            throw new AutorizacionException("Credenciales inválidas.");
        }

        var token = _tokenService.GenerarToken(usuario);
        return new LoginResponse(token);
    }
}
