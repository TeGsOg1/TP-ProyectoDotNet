using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Autorizacion;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites.EliminarTramite;

public class EliminarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IActualizacionEstadoExpedienteService _actualizacionEstadoExpedienteService;

    public EliminarTramiteUseCase (ITramiteRepository tramiteRepository, IActualizacionEstadoExpedienteService actualizacionEstadoExpedienteService, IAutorizacionService autorizacionService)
    {
        _tramiteRepository = tramiteRepository;
        _actualizacionEstadoExpedienteService = actualizacionEstadoExpedienteService;
        _autorizacionService = autorizacionService;
    }

    public EliminarTramiteResponse Ejecutar(EliminarTramiteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja)) {
            throw new AutorizacionException("El usuario no posee permiso para eliminar el tramite.");
        }
        var tramite =  _tramiteRepository.ObtenerTramiteId(request.Id);
        if (tramite is null) {
            throw new EntidadNoEncontradaException($"No se encontro el tramite con id {request.Id}");
        }
        Guid expedienteId = tramite.ExpedienteId;
        _tramiteRepository.EliminarTramite(request.Id);
        _actualizacionEstadoExpedienteService.Actualizar(request.IdUsuario, expedienteId);
        return new EliminarTramiteResponse(tramite.Id);
    }
}
