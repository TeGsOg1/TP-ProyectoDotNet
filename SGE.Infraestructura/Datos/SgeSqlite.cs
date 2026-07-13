using System.Data;
using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Enums;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura.Datos;

/// <summary>
/// Crea la base SQLite y garantiza los datos mínimos necesarios para usar la
/// aplicación. Es idempotente: puede ejecutarse más de una vez sin duplicar datos.
/// </summary>
public static class SgeSqlite
{
    public static void Inicializar(SgeContext context, IHashContrasenaService hashContrasenaService)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(hashContrasenaService);

        context.Database.EnsureCreated();
        EstablecerJournalModeDelete(context);

        AgregarUsuariosSemillaSiFaltan(context, hashContrasenaService);

        // La inicialización es el único punto de Infraestructura, ajeno a un
        // repositorio, que persiste directamente los datos semilla.
        context.SaveChanges();
    }

    private static void EstablecerJournalModeDelete(SgeContext context)
    {
        var connection = context.Database.GetDbConnection();
        var debeCerrarConexion = connection.State != ConnectionState.Open;

        if (debeCerrarConexion)
        {
            connection.Open();
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA journal_mode=DELETE;";
            command.ExecuteNonQuery();
        }
        finally
        {
            if (debeCerrarConexion)
            {
                connection.Close();
            }
        }
    }

private static void AgregarUsuariosSemillaSiFaltan(
        SgeContext context,
        IHashContrasenaService hashContrasenaService)
    {
        // 1. Administrador (Rubrica exige admin@sge.com y admin123 hasheado)
        if (!context.Usuarios.Any(usuario => usuario.CorreoElectronico == "admin@sge.com"))
        {
            context.Usuarios.Add(new Usuario(
                "Administrador SGE",
                "admin@sge.com",
                hashContrasenaService.Hashear("admin123"),
                esAdministrador: true));
        }

        // 2. Usuario Operador (Con permisos específicos)
        var operador = context.Usuarios.SingleOrDefault(usuario => usuario.CorreoElectronico == "operador@sge.com");
        if (operador is null)
        {
            operador = new Usuario(
                "Usuario operador",
                "operador@sge.com",
                hashContrasenaService.Hashear("operador123"));
            
            // ¡Magia DDD! Usamos tu método y EF Core lo pasa a JSON automáticamente.
            operador.AsignarPermiso(Permiso.ExpedienteAlta);
            operador.AsignarPermiso(Permiso.ExpedienteModificacion);
            operador.AsignarPermiso(Permiso.TramiteAlta);
            operador.AsignarPermiso(Permiso.TramiteModificacion);
            
            context.Usuarios.Add(operador);
        }

        // 3. Usuario Lector (Sin permisos)
        if (!context.Usuarios.Any(usuario => usuario.CorreoElectronico == "lector@sge.com"))
        {
            context.Usuarios.Add(new Usuario(
                "Usuario consulta",
                "lector@sge.com",
                hashContrasenaService.Hashear("lector123")));
        }
    }
}
