using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
namespace SGE.Aplicacion.Tramites.AgregarTramite;

public class AgregarTramiteUseCase 
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IActualizacionEstadoExpedienteService _actualizacionEstadoExpedienteService;

    public AgregarTramiteUseCase (ITramiteRepository tramiteRepository, IActualizacionEstadoExpedienteService actualizacionEstadoExpedienteService, IAutorizacionService autorizacionService)
    {
        _tramiteRepository = tramiteRepository;
        _actualizacionEstadoExpedienteService = actualizacionEstadoExpedienteService;
        _autorizacionService = autorizacionService;
    }

    public AgregarTramiteResponse Ejecutar (AgregarTramiteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteAlta))
        {
            throw new AutorizacionException("El usuario no posee permiso para dar de alta un tramites.");
        }
        Tramite tramite = new Tramite(
            request.Id,
            request.ExpedienteId,
            new ContenidoTramite(request.contenido),
            new EtiquetaTramite(request.etiqueta),
            request.IdUsuario
        );
        _tramiteRepository.AgregarTramite(tramite);

        _actualizacionEstadoExpedienteService.Actualizar(request.IdUsuario, tramite.ExpedienteId);

        return new AgregarTramiteResponse(tramite.Id);
    }
}