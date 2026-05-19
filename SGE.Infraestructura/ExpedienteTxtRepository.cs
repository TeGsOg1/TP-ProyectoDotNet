using System;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Enums;
using SGE.Dominio.ValueObjects;
namespace SGE.Infraestructura;

public class ExpedienteTxtRepository : IExpedienteRepository
{
    private readonly string _nombreArchivo = "expedientes.txt";

    public void Agregar(Expediente expediente) //solo agrego un expediente
    {
        //porque pongo true? porque quiero agregar al final del archivo, no sobrescribirlo cada vez que agrego 
        // un expediente nuevo
        using var sw = new StreamWriter(_nombreArchivo, true);
        sw.WriteLine(expediente.Id);
        sw.WriteLine(expediente.Caratula.Texto); 
        // el "O": como estoy escribiendo un DateTime, lo guardo en formato ISO 8601 (Un estandar internacional)
        // para facilitar su lectura y reconstrucción posterior
        sw.WriteLine(expediente.FechaCreacion.ToString("O")); 
        sw.WriteLine(expediente.FechaUltimaModificacion.ToString("O"));
        sw.WriteLine(expediente.UsuarioUltimoCambio);
        sw.WriteLine(expediente.Estado.ToString());
    }
    public void Eliminar(Guid id)
    {
        var expedientes = ObtenerTodosExpedientes().ToList();
        var expedienteAEliminar = expedientes.FirstOrDefault(e => e.Id == id);
        if (expedienteAEliminar == null)
        {
            throw new RepositorioException($"No se encontró el expediente con ID {id} para eliminar.");
        }
        expedientes.Remove(expedienteAEliminar);
        GuardarTodos(expedientes);
    }
    private void GuardarTodos(List<Expediente> expedientes)
    {
        //porque pongo false? porque quiero sobrescribir el archivo completo cada vez que guardo, no agregar al final
        using var sw = new StreamWriter(_nombreArchivo, false);
        foreach (var e in expedientes)
        {
            sw.WriteLine(e.Id);
            sw.WriteLine(e.Caratula.Texto);
            sw.WriteLine(e.FechaCreacion.ToString("O"));
            sw.WriteLine(e.FechaUltimaModificacion.ToString("O"));
            sw.WriteLine(e.UsuarioUltimoCambio);
            sw.WriteLine(e.Estado.ToString());
        }
    }
    public void Actualizar(Expediente expediente)
    {
        var expedientes = ObtenerTodosExpedientes().ToList();
        var indice = expedientes.FindIndex(e => e.Id == expediente.Id);
        //FindIndex devuelve -1 si no encuentra el elemento, por eso verifico esa condición para lanzar 
        // una excepción si no se encuentra el expediente a actualizar
        if (indice == -1)
        {
            throw new RepositorioException($"No se encontró el expediente con ID {expediente.Id} para actualizar.");
        }
        expedientes[indice] = expediente;
        GuardarTodos(expedientes);
    }
    public IEnumerable<Expediente> ObtenerTodosExpedientes()
    {
        var expedientes = new List<Expediente>();
        if (!File.Exists(_nombreArchivo)) 
        {
            return expedientes;
        }
        using var sr = new StreamReader(_nombreArchivo);
        while (!sr.EndOfStream)
        {
            var id = Guid.Parse(sr.ReadLine() ?? "");
            var caratulaTexto = sr.ReadLine() ?? "";
            var fechaCreacion = DateTime.Parse(sr.ReadLine() ?? "");
            var fechaModif = DateTime.Parse(sr.ReadLine() ?? "");
            var usuario = Guid.Parse(sr.ReadLine() ?? "");
            var estado = Enum.Parse<Estado>(sr.ReadLine() ?? "");

            // Reconstruyo el Value Object y la Entidad
            var caratula = new Caratula(caratulaTexto);
            //Factory Method para reconstruir el expediente con todos sus datos (incluyendo fechas y estado)
            var expediente = Expediente.Reconstruct(id, caratula, fechaCreacion, fechaModif, usuario, estado);   
            expedientes.Add(expediente);
        }
        return expedientes;
    }
    public Expediente? ObtenerExpedientePorId(Guid id)
    {
        return ObtenerTodosExpedientes().FirstOrDefault(e => e.Id == id);
    }
}
