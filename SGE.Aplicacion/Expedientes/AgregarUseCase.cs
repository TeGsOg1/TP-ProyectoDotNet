using System;
using SGE.Aplicacion; 
using SGE.Dominio;
namespace SGE.Aplicacion.Expedientes;

public class AgregarUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IAutorizacionService _autorizacionService;

    public AgregarUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
    }

    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteCreacion))
        {
           throw new AutorizacionException("El usuario no tiene permiso para agregar expedientes.");
        }
        // 2. Instanciación (El Dominio valida que no esté vacío)
        var caratula = new Caratula(request.Caratula);   
        var nuevoExpediente = new Expediente(caratula, request.IdUsuario);

        // 3. Persistencia. El repositorio se encarga de todo lo demas
        _expedienteRepository.Agregar(nuevoExpediente);

        return new AgregarExpedienteResponse(nuevoExpediente.Id);
    }
}
