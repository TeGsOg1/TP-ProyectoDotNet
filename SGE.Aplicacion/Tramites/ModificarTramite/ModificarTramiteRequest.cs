namespace SGE.Aplicacion.Tramites.ModificarTramite;

public record ModificarTramiteRequest(
    Guid Id,
    string Contenido,
    int Etiqueta,
    Guid IdUsuario
);