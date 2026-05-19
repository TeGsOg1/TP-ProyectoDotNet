using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites.AgregarTramite;
using SGE.Aplicacion.Tramites.ListarTramites;
using SGE.Dominio.Comun;
using SGE.Infraestructura;
using SGE.Infraestructura.Autorizacion;

namespace SGE.Consola;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Sistema de Gestion de Expedientes\n");
        
        var expedienteRepository = new ExpedienteTxtRepository();
        var tramiteRepository = new TramiteTxtRepository();
        
        IAutorizacionService autorizacionService = new AutorizacionProvisionalService();
        var actualizacionEstadoService = new ActualizacionEstadoExpedienteService(
            expedienteRepository,
            tramiteRepository
        );

        var agregarExpedienteUseCase = new AgregarUseCase(
            expedienteRepository,
            autorizacionService
        );

        var agregarTramiteUseCase = new AgregarTramiteUseCase(
            tramiteRepository,
            actualizacionEstadoService,
            autorizacionService
        );

        var listarTramitesUseCase = new ListarTramitesUseCase(tramiteRepository);
        var obtenerExpedientesUseCase = new ObtenerTodosExpedientesUseCase(expedienteRepository);

        Guid usuarioId = Guid.NewGuid();

        try
        {
            
            //Crear expediente
            var altaRequest = new AgregarExpedienteRequest(
                "Compra de insumos informáticos",
                usuarioId
            );

            var altaResponse = agregarExpedienteUseCase.Ejecutar(altaRequest);
            Guid expedienteId = altaResponse.Id;

            Console.WriteLine($"Expediente creado OK -> ID: {expedienteId}\n");
            
            //Agregar trámites
            var tramite1 = new AgregarTramiteRequest(
                Guid.NewGuid(),
                expedienteId,
                "Se inicia el expediente",
                "INICIADO",
                usuarioId
            );

            agregarTramiteUseCase.Ejecutar(tramite1);
            Console.WriteLine("Trámite iniciado agregado");

            var tramite2 = new AgregarTramiteRequest(
                Guid.NewGuid(),
                expedienteId,
                "Se envía a revisión",
                "EN_REVISION",
                usuarioId
            );

            agregarTramiteUseCase.Ejecutar(tramite2);
            Console.WriteLine("Trámite en revision agregado\n");
            
            //Listar trámites del expediente
            var listarRequest = new ListarTramiteRequest(expedienteId);
            var tramites = listarTramitesUseCase.Ejecutar(listarRequest);

            Console.WriteLine("TRAMITES DEL EXPEDIENTE:");
            foreach (var t in tramites.Tramites)
            {
                Console.WriteLine($"- {t.Etiqueta} | {t.Contenido} | {t.FechaCreacion}");
            }
            
            //Listar expedientes
            var expedientes = obtenerExpedientesUseCase.Ejecutar();

            Console.WriteLine("\nEXPEDIENTES:");
            foreach (var e in expedientes.Expedientes)
            {
                Console.WriteLine($"- {e.Id} | {e.Caratula} | Estado: {e.Estado}");
            }

        }
        catch (DominioException ex)
        {
            Console.WriteLine($"\n[ERROR DE DOMINIO] {ex.Message}");
        }
        catch (AutorizacionException ex)
        {
            Console.WriteLine($"\n[ERROR DE AUTORIZACION] {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERROR GENERAL] {ex.Message}");
        }
        

        Console.WriteLine("\n\n----- PRUEBAS DE ERROR -----");

        //Error de dominio: carátula vacía
        try
        {
            Console.WriteLine("\nIntentando crear expediente con carátula vacía...");
            var errorRequest = new AgregarExpedienteRequest("", usuarioId);
            agregarExpedienteUseCase.Ejecutar(errorRequest);
        }
        catch (DominioException ex)
        {
            Console.WriteLine($"OK -> DominioException capturada: {ex.Message}");
        }

        //Error de autorización (simulación)
        try
        {
            Console.WriteLine("\nPara probar AutorizacionException:");
            Console.WriteLine("Cambiar temporalmente AutorizacionProvisionalService a 'return false'");
            Console.WriteLine("y volver a ejecutar el programa.");
        }
        catch (AutorizacionException ex)
        {
            Console.WriteLine($"OK -> AutorizacionException capturada: {ex.Message}");
        }

        Console.WriteLine("\nFin del programa.");
    }
}
