using System;
using SGE.Aplicacion.Expedientes; 
using SGE.Aplicacion.Tramites;
using SGE.Dominio;
using System.Linq;

namespace SGE.Aplicacion;

public class ActualizacionEstadoExpedienteService : IActualizacionEstadoExpedienteService
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly ITramiteRepository _tramiteRepository;

    public ActualizacionEstadoExpedienteService(IExpedienteRepository expedienteRepository, ITramiteRepository tramiteRepository)
    {
        _expedienteRepository = expedienteRepository;
        _tramiteRepository = tramiteRepository;
    }

    public void Actualizar (Guid IdUsuario, Guid IdExpediente)
    {
        var expediente = _expedienteRepository.ObtenerExpedientePorId(IdExpediente);
        if(expediente == null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado");
        }
        var tramites = _tramiteRepository.ObtenerTramitesPorExpediente(IdExpediente);
        tramites = tramites.OrderByDescending(t => t.FechaCreacion) ;   
        var Ultimo_tramite = tramites.FirstOrDefault();
        //El tp pide que si es null se ponga de nuevo en recieniniciado
        var etiquetaTramite = Ultimo_tramite?.Etiqueta;
        bool cambio = expediente.ActualizarEstado(etiquetaTramite, IdUsuario);
        if (cambio)
        {
            _expedienteRepository.Actualizar(expediente);
        }
    }
}
