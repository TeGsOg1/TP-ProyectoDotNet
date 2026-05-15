namespace SGE.Aplicacion.Tramites;
public record TramiteDTO (
Guid TramiteId,
Guid ExpedienteId,
ContenidoTramite contenido,
EtiquetaTramite etiqueta,
DateTime fechaCreacion,
DateTime fechaModificacion);