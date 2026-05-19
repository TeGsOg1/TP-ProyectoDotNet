using System;
using SGE.Dominio.ValueObjects;
using SGE.Dominio.Enums;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; }
    public Caratula Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public Estado Estado { get; private set; }

    public Expediente(Caratula caratula, Guid usuarioCreacion)
    {
        if (caratula is null) throw new DominioException("Carátula es obligatoria.");
        if (usuarioCreacion == Guid.Empty) throw new DominioException("Usuario de creación inválido.");

        Id = Guid.NewGuid();
        Caratula = caratula;
        FechaCreacion = DateTime.UtcNow;
        FechaUltimaModificacion = FechaCreacion;
        UsuarioUltimoCambio = usuarioCreacion;
        Estado = Estado.RecienIniciado;
    }

    private Expediente() { }

    public static Expediente Reconstruct(Guid id, Caratula caratula, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio, Estado estado)
    {
        if (id == Guid.Empty) throw new DominioException("Id inválido.");
        if (caratula is null) throw new DominioException("Carátula es obligatoria.");
        if (usuarioUltimoCambio == Guid.Empty) throw new DominioException("Usuario último cambio inválido.");
        if (fechaUltimaModificacion < fechaCreacion) throw new DominioException("FechaUltimaModificacion no puede ser menor que FechaCreacion.");

        return new Expediente
        {
            Id = id,
            Caratula = caratula,
            FechaCreacion = fechaCreacion,
            FechaUltimaModificacion = fechaUltimaModificacion,
            UsuarioUltimoCambio = usuarioUltimoCambio,
            Estado = estado
        };
    }

    public void ActualizarCaratula(Caratula nuevaCaratula, Guid usuarioId)
    {
        if (nuevaCaratula is null) throw new DominioException("Carátula es obligatoria.");
        if (usuarioId == Guid.Empty) throw new DominioException("Usuario inválido.");

        Caratula = nuevaCaratula;
        FechaUltimaModificacion = DateTime.UtcNow;
        if (FechaUltimaModificacion < FechaCreacion) FechaUltimaModificacion = FechaCreacion;
        UsuarioUltimoCambio = usuarioId;
    }

    public void ModificarCaratula(Caratula nuevaCaratula, Guid idUsuario)
    {
        ActualizarCaratula(nuevaCaratula, idUsuario);
    }

    public bool ActualizarEstado(Enums.EtiquetaTramite? ultimaEtiqueta, Guid idUsuario)
    {
        if (idUsuario == Guid.Empty) throw new DominioException("Usuario inválido.");

        Estado? nuevoEstado = null;

        if (ultimaEtiqueta is null)
        {
            nuevoEstado = Estado.RecienIniciado;
        }
        else
        {
            switch (ultimaEtiqueta.Value)
            {
                case Enums.EtiquetaTramite.Resolucion:
                    nuevoEstado = Estado.ConResolucion;
                    break;
                case Enums.EtiquetaTramite.PaseAEstudio:
                    nuevoEstado = Estado.ParaResolver;
                    break;
                case Enums.EtiquetaTramite.PaseAlArchivo:
                    nuevoEstado = Estado.Finalizado;
                    break;
                default:
                    nuevoEstado = null;
                    break;
            }
        }

        if (nuevoEstado is null || nuevoEstado.Value == Estado)
            return false;

        Estado = nuevoEstado.Value;
        FechaUltimaModificacion = DateTime.UtcNow;
        if (FechaUltimaModificacion < FechaCreacion) FechaUltimaModificacion = FechaCreacion;
        UsuarioUltimoCambio = idUsuario;
        return true;
    }

    public void CambiarEstado(Estado nuevoEstado, Guid usuarioId)
    {
        if (!Enum.IsDefined(typeof(Estado), nuevoEstado)) throw new DominioException("Estado inválido.");
        if (usuarioId == Guid.Empty) throw new DominioException("Usuario inválido.");

        Estado = nuevoEstado;
        FechaUltimaModificacion = DateTime.UtcNow;
        if (FechaUltimaModificacion < FechaCreacion) FechaUltimaModificacion = FechaCreacion;
        UsuarioUltimoCambio = usuarioId;
    }
}