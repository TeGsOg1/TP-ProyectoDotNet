namespace SGE.Aplicacion.Tramites;
public record TramiteDTO (Guid TramiteId,
Guid ExpedienteId,
string contenido,
int etiqueta,
DateTime fechaCreacion,
DateTime fechaModificacion,
Guid IdUsuario,
Guid UsuarioUltimoCambio);
