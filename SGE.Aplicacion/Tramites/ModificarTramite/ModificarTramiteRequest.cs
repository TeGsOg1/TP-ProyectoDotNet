
namespace SGE.Aplicacion.Tramites.ModificarTramite;

public record ModificarTramiteRequest(
    Guid Id,
    string Contenido,
    string Etiqueta,
    Guid IdUsuario
);