using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio;
using SGE.Dominio.Comun;
using SGE.Dominio.Enums;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ActualizarEstadoExpedienteUseCase
{
    private readonly IExpedienteRepository _repository;
    private readonly IAutorizacionService _autorizacionService;
    public ActualizarEstadoExpedienteUseCase(IExpedienteRepository repository, IAutorizacionService autorizacionService)
    {
        _repository = repository;
        _autorizacionService = autorizacionService;
    }
    public void Ejecutar(ModificarEstadoExpedienteRequest request)
    {
        // 1. Autorización. Tenemos que crear el enumerativo de perimisos y asignarlo a los usuarios para poder validar aca
        if(!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permiso para modificar el estado del expediente.");
        }
        // Recupero la Entidad
        var expediente = _repository.ObtenerExpedientePorId(request.IdExpediente);
        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado.");
        }
        // trato de parsear el estado nuevo, el true lo hago para que ignore el case
        if (!Enum.TryParse<Estado>(request.EstadoNuevo, true, out var nuevoEstadoParseado))
        {
            throw new DominioException("El estado ingresado no es válido para el negocio.");
        }
        expediente.CambiarEstado(nuevoEstadoParseado, request.IdUsuario);
        _repository.Actualizar(expediente);
    }
}
