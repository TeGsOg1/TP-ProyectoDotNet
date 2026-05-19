# Probá el programa

Qué hace este programa:

Este programa es una consola de ejemplo para gestionar "expedientes" y sus "trámites". Permite crear un expediente, agregar trámites asociados, listar trámites y expedientes y mostrar cómo se manejan errores de dominio y autorización (esta última simulada). Está pensado como un ejercicio práctico hecho por alumnos.

Cómo arrancar (copiá y pegá en PowerShell):

```powershell
dotnet run --project SGE.Consola/SGE.Consola.csproj
```

Qué hace el ejemplo:
- Crea un expediente con una carátula.
- Agrega dos trámites al expediente.
- Lista los trámites y los expedientes guardados.
- Hace dos pruebas de error: carátula vacía (error de dominio) y una nota para simular error de autorización.

Fragmento de código útil (sacado de `Program.cs`):

```csharp
// Creación básica
var expedienteRepository = new ExpedienteTxtRepository();
var tramiteRepository = new TramiteTxtRepository();
IAutorizacionService autorizacionService = new AutorizacionProvisionalService();

var agregarExpedienteUseCase = new AgregarUseCase(expedienteRepository, autorizacionService);

var altaRequest = new AgregarExpedienteRequest("Compra de insumos informáticos", usuarioId);
var altaResponse = agregarExpedienteUseCase.Ejecutar(altaRequest);
Guid expedienteId = altaResponse.Id;
Console.WriteLine($"Expediente creado OK -> ID: {expedienteId}");
```

Ejemplo de salida que vas a ver por consola (los GUID y fechas van a cambiar):

```
Sistema de Gestion de Expedientes

Expediente creado OK -> ID: 3f1e2d4a-9c2b-4f3a-8a2a-6b9c0d1e2f3a
Trámite iniciado agregado
Trámite en revision agregado

TRAMITES DEL EXPEDIENTE:
- INICIADO | Se inicia el expediente | 2026-05-19T12:34:56
- EN_REVISION | Se envía a revisión | 2026-05-19T12:35:01

EXPEDIENTES:
- 3f1e2d4a-9c2b-4f3a-8a2a-6b9c0d1e2f3a | Compra de insumos informáticos | Estado: INICIADO


----- PRUEBAS DE ERROR -----

Intentando crear expediente con carátula vacía...
OK -> DominioException capturada: La carátula no puede ser vacía.

Para probar la parte de autorización (si querés ver el error):
1) Abrí `SGE.Infraestructura/AutorizacionProvisionalService.cs`
2) Cambiá el "return true;" por "return false;"
3) Volvé a correr el programa y vas a ver la excepción de autorización.

Fin.