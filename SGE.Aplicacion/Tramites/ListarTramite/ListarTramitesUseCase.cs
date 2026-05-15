using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.ListarTramites;

public class ListarTramitesUseCase 
{
    private readonly ITramiteRepository _tramiteRepository;

    public ListarTramitesUseCase (ITramiteRepository tramiteRepository)
    {
        _tramiteRepository = tramiteRepository;
    }

    public ListarTramitesRespose Ejecutar (ListarTramiteRequest request)
    {
        IEnumerable<Tramite> tramites = _tramiteRepository.ObtenerTramitesPorExpediente(request.ExpedienteId);

        List<TramiteDTO> tramitesDTO = new();
        
        foreach (Tramite tramite in tramites)
        {
            tramitesDTO dto = new(
                tramite.TramiteId,
                tramite.ExpedienteId,
                tramite.Contenido,
                tramite.Etiqueta,
                tramite.FechaCreacion,
                tramite.FechaModificacion,
                tramite.IdUsuario,
                tramite.UsuarioUltimoCambio);
            
            tramitesDTO.Add(dto);
        }
        
        return new ListarTramitesRespose (tramitesDTO);
    }
}