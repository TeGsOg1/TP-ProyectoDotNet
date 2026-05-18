using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Comun;

namespace SGE.Infraestructura;

public class TramiteTxtRepository : ITramiteRepository
{
    private readonly string _nombreArchivo = "tramites.txt";
    public void AgregarTramite(Tramite tramite) {

        using var sw = new StreamWriter(_nombreArchivo, true);
        sw.WriteLine($"{tramite.TramiteId}|{tramite.ExpedienteId}|{tramite.contenido}|{tramite.etiqueta}|{tramite.fechaCreacion}|{tramite.fechaModificacion}|{tramite.UsuarioUltimoCambio}");
    }

    public IEnumerable<Tramite> ObtenerTodos()
    {
        var lista = new List<Tramite>();
        if (!File.Exists(_nombreArchivo)) 
            return lista;

        foreach (var linea in File.ReadAllLines(_nombreArchivo))
        {
            if (string.IsNullOrWhiteSpace(linea)) 
                continue;

            var datos = linea.Split('|');
            if (datos.Length != 7)
                continue;
                
            var tramite = Tramite.Reconstruir(
                Guid.Parse(datos[0]),
                Guid.Parse(datos[1]),
                new ContenidoTramite(datos[2]),
                Enum.Parse<EtiquetaTramite>(datos[3]),
                DateTime.Parse(datos[4]),
                DateTime.Parse(datos[5]),
                Guid.Parse(datos[6])
            );
            lista.Add(tramite);
        }
        return lista;
    }

    public Tramite? ObtenerTramiteId(Guid id)
    {
        return ObtenerTodos().FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Tramite> ObtenerTramitesPorExpediente(Guid expedienteId)
    {
        return ObtenerTodos().Where(t => t.ExpedienteId == expedienteId);
    }

    public void ModificarTramite(Tramite t)
    {
        var todos = ObtenerTodos().ToList();
        var indice = todos.FindIndex(tramite => tramite.Id == t.Id);
        if (indice == -1)
        {
            throw new RepositorioException($"No se encontró el trámite con ID {t.Id} para modificar.");
        }
        todos[indice] = t;
        GuardarTodos(todos);
    }

    public void EliminarTramite(Guid id)
    {
        var todos = ObtenerTodos().ToList();
        var tramite = todos.FirstOrDefault(t => t.Id == id);
        if (tramite == null)
        {
            throw new RepositorioException($"No se encontró el trámite con ID {id} para eliminar.");
        }
        todos.Remove(tramite);
        GuardarTodos(todos);
    }

    public void EliminarTramitePorExpediente(Guid expedienteId)
    {
        var todos = ObtenerTodos().ToList();
        todos.RemoveAll(t => t.ExpedienteId == expedienteId);
        GuardarTodos(todos);
    }

    private void GuardarTodos(List<Tramite> tramites)
    {
        using var sw = new StreamWriter(_nombreArchivo, false);
        foreach (var t in tramites)
        {
            sw.WriteLine($"{t.Id}|{t.ExpedienteId}|{t.Contenido}|{t.Etiqueta}|{t.FechaCreacion}|{t.FechaModificacion}|{t.UsuarioUltimoCambio}");
        }
    }
}