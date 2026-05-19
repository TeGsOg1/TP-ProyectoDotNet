using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites.AgregarTramite;
using SGE.Aplicacion.Tramites.ListarTramite;
using SGE.Dominio.Comun;
using SGE.Infraestructura;

Console.WriteLine("=== SISTEMA GESTION DE EXPEDIENTES ===");
Console.WriteLine();

try
{
    var expedienteRepo = new ExpedienteTxtRepository();
    var tramiteRepo = new TramiteTxtRepository();

    IAutorizacionService autorizacionService = new AutorizacionProvisionalService();

    var agregarExpedienteUC = new AgregarUseCase(expedienteRepo);
    var obtenerExpedientesUC = new ObtenerTodosExpedientesUseCase(expedienteRepo);
    var agregarTramiteUC = new AgregarTramiteUseCase(tramiteRepo, expedienteRepo, autorizacionService);
    var listarTramitesUC = new ListarTramitesUseCase(tramiteRepo);

    // CAMINO OK
    Console.WriteLine("---- CAMINO FELIZ ----");

    var altaExpediente = new AgregarExpedienteRequest("Compra de inmueble");
    var expedienteCreado = agregarExpedienteUC.Execute(altaExpediente);

    Console.WriteLine($"Expediente creado OK -> ID: {expedienteCreado.Id}");
    Console.WriteLine();

    var tramiteRequest = new AgregarTramiteRequest(
        expedienteCreado.Id,
        "Presentación de documentación inicial"
    );

    var tramiteResponse = agregarTramiteUC.Execute(tramiteRequest);

    Console.WriteLine($"Trámite agregado OK -> ID: {tramiteResponse.Id}");
    Console.WriteLine();

    var expedientes = obtenerExpedientesUC.Execute(new ObtenerTodosExpedientesRequest());
    Console.WriteLine("Listado de expedientes:");
    foreach (var exp in expedientes.Expedientes)
    {
        Console.WriteLine($" - {exp.Id} | {exp.Caratula} | Estado: {exp.Estado}");
    }

    Console.WriteLine();

    var tramites = listarTramitesUC.Execute(new ListarTramiteRequest(expedienteCreado.Id));
    Console.WriteLine("Trámites del expediente:");
    foreach (var t in tramites.Tramites)
    {
        Console.WriteLine($" - {t.Id} | {t.Contenido}");
    }

    // CAMINOS DE ERROR
    Console.WriteLine();
    Console.WriteLine("---- CAMINOS DE ERROR ----");

    // Error de Dominio → carátula vacía
    try
    {
        Console.WriteLine("Intentando crear expediente con carátula vacía...");
        agregarExpedienteUC.Execute(new AgregarExpedienteRequest(""));
    }
    catch (DominioException ex)
    {
        Console.WriteLine($"ERROR DE DOMINIO: {ex.Message}");
    }

    Console.WriteLine();

    //Error de Autorización
    try
    {
        Console.WriteLine("Simulando error de autorización...");

        IAutorizacionService authFailService = new AutorizacionProvisionalService(false);
        var agregarTramiteSinPermisoUC =
            new AgregarTramiteUseCase(tramiteRepo, expedienteRepo, authFailService);

        agregarTramiteSinPermisoUC.Execute(
            new AgregarTramiteRequest(expedienteCreado.Id, "Trámite sin permiso")
        );
    }
    catch (AutorizacionException ex)
    {
        Console.WriteLine($"ERROR DE AUTORIZACION: {ex.Message}");
    }
}
catch (RepositorioException ex)
{
    Console.WriteLine($"ERROR DE REPOSITORIO: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR GENERAL: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("Fin del programa...");
Console.ReadKey();
