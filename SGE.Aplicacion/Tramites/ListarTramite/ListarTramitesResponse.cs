
namespace SGE.Aplicacion.Tramites.ListarTramites;

public record ListarTramitesResponse(
        IEnumerable<TramiteDTO> Tramites
);
