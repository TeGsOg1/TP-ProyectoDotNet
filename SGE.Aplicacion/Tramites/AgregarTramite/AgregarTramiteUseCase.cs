using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Dominio.Enums;
using SGE.Dominio.ValueObjects;

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
            throw new AutorizacionException("El usuario no posee permiso para dar de alta un tramite.");
        }
        
        Tramite tramite = new Tramite(
            request.ExpedienteId,
            Enum.Parse<EtiquetaTramite>(request.etiqueta ?? "", true),
            new ContenidoTramite(request.contenido ?? ""),
            request.IdUsuario
        );
        
        _tramiteRepository.AgregarTramite(tramite);

        _actualizacionEstadoExpedienteService.Actualizar(request.IdUsuario, tramite.ExpedienteId);

        return new AgregarTramiteResponse(tramite.Id);
    }
}
