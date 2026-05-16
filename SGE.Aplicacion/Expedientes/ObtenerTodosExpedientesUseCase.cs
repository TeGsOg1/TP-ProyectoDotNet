using System;
using System.Linq;
using SGE.Aplicacion;
using SGE.Dominio;
namespace SGE.Aplicacion.Expedientes;

public class ObtenerTodosExpedientesUseCase
{
    private readonly IExpedienteRepository _repository;
   public ObtenerTodosExpedientesUseCase(IExpedienteRepository repository)
    {
        _repository = repository;
    }
    public ObtenerTodosExpedienteResponse Ejecutar()
    {
        var expedientes =  _repository.ObtenerTodosExpedientes();   
        var expedientesDTO = expedientes.Select(e => new ExpedienteDTO(
            e.Id,
            //Caratula es un Value Object, por lo que para obtener su contenido tengo que acceder a la propiedad Contenido
            e.Caratula.Contenido, 
            e.FechaCreacion,
            e.FechaUltimaModificacion,
            e.UsuarioUltimoCambio,
            e.Estado.ToString()
        )).ToList();
        return new ObtenerTodosExpedienteResponse(expedientesDTO);
    }
}

