using SGE.Dominio.Enums;
using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio;
using SGE.Dominio.Comun;
using SGE.Dominio.ValueObjects;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ActualizarCaratulaExpedienteUseCase
{
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IExpedienteRepository _repository;
    private readonly IAutorizacionService _autorizacionService;
    public ActualizarCaratulaExpedienteUseCase(IExpedienteRepository repository, IAutorizacionService autorizacionService, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _unidadDeTrabajo = unidadDeTrabajo;
        _repository = repository;
        _autorizacionService = autorizacionService;
    }
    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {
        // 2. Perfecto el chequeo de permisos acá
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permiso para modificar la carátula.");
        }
        var expediente = _repository.ObtenerExpedientePorId(request.IdExpediente);
        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado.");
        }
        var nuevaCaratula = new Caratula(request.NuevaCaratula);
        
        // El dominio valida sus propias reglas
        expediente.ActualizarCaratula(nuevaCaratula, request.IdUsuario);
        // usamos el UoW
        _unidadDeTrabajo.Guardar();

        return new ModificarCaratulaExpedienteResponse();
    }
}
