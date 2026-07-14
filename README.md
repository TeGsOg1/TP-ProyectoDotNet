# SGE - Sistema de Gestión de Expedientes

Este repositorio contiene una API Web para gestionar usuarios, expedientes y trámites.

La aplicación principal actual está en `SGE.WebAPI` y usa:
- ASP.NET Core Minimal APIs
- Autenticación JWT
- SQLite para persistencia local
- Manejo global de excepciones con `ProblemDetails`

## Cómo ejecutar

Ejecuta esto desde la carpeta SGE.WEBApi del repositorio:

```powershell
dotnet run
```

Después de arrancar, la API estará disponible en `https://localhost:5134`.

## Endpoints principales

### Autenticación

- `POST /api/auth/login`
  - Inicia sesión y devuelve un token JWT.
- `POST /api/auth/registrar`
  - Registra un nuevo usuario.

### Usuarios

- `GET /api/usuarios/`
  - Lista todos los usuarios.
  - Requiere un token JWT válido y que el usuario sea administrador.
- `GET /api/usuarios/{id}`
  - Obtiene un usuario por su `guid`.
- `DELETE /api/usuarios/{id}`
  - Elimina un usuario.
- `PUT /api/usuarios/{id}/permisos`
  - Modifica permisos de un usuario.
- `PUT /api/usuarios/mis-datos`
  - Modifica los datos del usuario autenticado.

## Nota para el profesor

Si querés listar los usuarios sin iniciar sesion vas a tener que eliminar el codigo ".RequireAuthorization()" del endpoint y eliminar el parametro de entrada del metodo ListarUsuariosRequest y ListarUsuariosUseCase para que no hayan conflictos de accesos inautorizados.

## Autorización y tokens

Todos los endpoints de `usuarios` requieren autenticación.

Para consumirlos debes:
1. Hacer `POST /api/auth/login` con credenciales válidas.
2. Copiar el token JWT de la respuesta.
3. Enviar el header:

```http
Authorization: Bearer <token>
```

Si no envías un token válido, la API devolverá `401 Unauthorized`.
Si el usuario no tiene permisos de administrador, devolverá `403 Forbidden`.

## Cómo probar en local

- Abre Swagger o la documentación OpenAPI si estás en desarrollo.
- Usa `POST /api/auth/login` para obtener el token.
- Usa ese token en las solicitudes a `/api/usuarios/`.

## Notas de implementación

- La API usa el middleware global `ExcepcionGlobalMiddleware` para traducir excepciones de dominio en respuestas HTTP adecuadas.
- `SGE.Aplicacion` contiene los casos de uso, modelos y peticiones.
- `SGE.Infraestructura` contiene la implementación del repositorio y la configuración de datos.

## Estructura de carpetas relevante

- `SGE.WebAPI/` - proyecto de la API.
- `SGE.Aplicacion/` - lógica de negocio y casos de uso.
- `SGE.Infraestructura/` - persistencia y servicios de infraestructura.

