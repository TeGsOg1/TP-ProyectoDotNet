namespace SGE.Aplicacion.Tramites;
public record TramiteDTO (
Guid TramiteId,
Guid ExpedienteId,
string? contenido,
string? etiqueta,
DateTime fechaCreacion,
DateTime fechaModificacion,
Guid UsuarioUltimoCambio);
