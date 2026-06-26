using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Enums;
using SGE.Dominio.Comun;
using SGE.Dominio.ValueObjects;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IAutorizacionService _autorizacionService;

    public AltaExpedienteUseCase(IExpedienteRepository expedienteRepository, IUnidadDeTrabajo unidadDeTrabajo, IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
        _autorizacionService = autorizacionService;
    }

    public AltaExpedienteResponse Ejecutar(AltaExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
        {
            throw new AutorizacionException("El usuario no tiene permiso para crear expedientes.");
        }
        //armamos el Value Object
        var caratula = new Caratula(request.Caratula);
        // el constructor genera el Guid, setea la fecha y el Estado en RecienIniciado
        var nuevoExpediente = new Expediente(caratula, request.IdUsuario);
        // el repo agrega el expediente a la db
        _expedienteRepository.Agregar(nuevoExpediente);
        // terminamos de confirmar lo que hizo el repositorio antes y guardamos
        _unidadDeTrabajo.Guardar();
        return new AltaExpedienteResponse(nuevoExpediente.Id);
    }
}
