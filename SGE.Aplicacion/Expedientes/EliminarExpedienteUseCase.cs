using System;
using SGE.Aplicacion.Tramites; 
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio;
using SGE.Dominio.Enums;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;
//AL ELIMINAR UN EXPEDIENTE TENGO QUE ELIMINAR TODOS LOS TRÁMITES ASOCIADOS
public class EliminarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    public EliminarExpedienteUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService, ITramiteRepository tramiteRepository)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
        _tramiteRepository = tramiteRepository;
    }
    public EliminarExpedienteResponse Ejecutar(EliminarExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
        {
            throw new AutorizacionException("El usuario no tiene permiso para eliminar expedientes.");
        }
        var expediente = _expedienteRepository.ObtenerExpedientePorId(request.Id);
        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado.");
        }
        var tramites = _tramiteRepository.ObtenerTramitesPorExpediente(request.Id);
        foreach (var tramite in tramites)
        {
            _tramiteRepository.EliminarTramite(tramite.Id);
        }
        _expedienteRepository.Eliminar(request.Id);
        return new EliminarExpedienteResponse();
    }
}
