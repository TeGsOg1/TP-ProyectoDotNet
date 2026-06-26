using System;
//este nuget lo tienen que desgargar desde apliacion en la consola para que funcione el AddScoped
//dotnet add package Microsoft.Extensions.DependencyInjection.Abstractions
using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion.Expedientes; 

namespace SGE.Aplicacion;

public static class Extensiones
{
    public static IServiceCollection AddAplicacion(this IServiceCollection servicios)
    {
        // Casos de Uso de Expedientes
        servicios.AddScoped<AltaExpedienteUseCase>();
        servicios.AddScoped<ActualizarCaratulaExpedienteUseCase>();
        servicios.AddScoped<ActualizarEstadoExpedienteUseCase>();
        servicios.AddScoped<EliminarExpedienteUseCase>();
        servicios.AddScoped<ObtenerExpedientePorIdUseCase>();
        servicios.AddScoped<ObtenerTodosExpedientesUseCase>();

        // aca van tus casos valen
        //
        
        return servicios;
    }
}
