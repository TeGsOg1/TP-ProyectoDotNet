using SGE.Aplicacion.Autorizacion;
namespace SGE.Aplicacion.Tramites.ModificarTramite;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService; 
    private readonly IActualizacionEstadoExpedienteService _actualizacionEstadoExpedienteService;

    public ModificarTramiteUseCase (ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService, IActualizacionEstadoExpedienteService actualizacionEstadoExpedienteService) {
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _actualizacionEstadoExpedienteService = actualizacionEstadoExpedienteService;
    }
    public ModificarTramiteResponse Ejecutar (ModificarTramiteRequest request) {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteModificacion)) {
            throw new AutorizacionException("El usuario no posee permiso para modificar el tramite.");
        }
        var tramite = _tramiteRepository.ObtenerTramiteId(request.Id);
        if (tramite is null) {
            throw new EntidadNoEncontradaException($"No se encontro el tramite con ID: {request.Id}");
        }
        tramite.Modificar(request.Contenido, request.Etiqueta, request.IdUsuario);
        _tramiteRepository.ModificarTramite(tramite);
        _actualizacionEstadoExpedienteService.Actualizar(request.IdUsuario, tramite.ExpedienteId);
        return new ModificarTramiteResponse(tramite.Id);       
    }
}