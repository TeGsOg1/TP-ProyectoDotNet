using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio;
using SGE.Dominio.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Dominio.ValueObjects;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ActualizarCaratulaExpedienteUseCase
{
    private readonly IExpedienteRepository _repository;
    private readonly IAutorizacionService _autorizacionService;
    public ActualizarCaratulaExpedienteUseCase(IExpedienteRepository repository, IAutorizacionService autorizacionService)
    {
        _repository = repository;
        _autorizacionService = autorizacionService;
    }
    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {
         if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permiso para modificar la carátula del expediente.");
        }
        var expediente = _repository.ObtenerExpedientePorId(request.IdExpediente);
        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado.");
        }
        //Creo el valueObject con la nueva carátula; el ValueObject validará su contenido
        var nuevaCaratula = new Caratula(request.NuevaCaratula);
        //tiene que estar en el dominio en la entidad
        expediente.ActualizarCaratula(nuevaCaratula, request.IdUsuario);
        _repository.Actualizar(expediente);
        return new ModificarCaratulaExpedienteResponse();
    }
}
