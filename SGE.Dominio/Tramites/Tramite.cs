using System;
using SGE.Dominio.ValueObjects;
using SGE.Dominio.Enums;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class Tramite
{
	public Guid Id { get; private set; }
	public Guid ExpedienteId { get; private set; }
	public EtiquetaTramite Etiqueta { get; private set; }
	public ContenidoTramite Contenido { get; private set; }
	public DateTime FechaCreacion { get; private set; }
	public DateTime FechaUltimaModificacion { get; private set; }
	public Guid UsuarioUltimoCambio { get; private set; }

	public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioCreacion)
	{
		if (expedienteId == Guid.Empty) throw new DominioException("ExpedienteId inválido.");
		if (!Enum.IsDefined(typeof(EtiquetaTramite), etiqueta)) throw new DominioException("Etiqueta inválida.");
		if (contenido is null) throw new DominioException("Contenido es obligatorio.");
		if (usuarioCreacion == Guid.Empty) throw new DominioException("Usuario de creación inválido.");

		Id = Guid.NewGuid();
		ExpedienteId = expedienteId;
		Etiqueta = etiqueta;
		Contenido = contenido;
		FechaCreacion = DateTime.UtcNow;
		FechaUltimaModificacion = FechaCreacion;
		UsuarioUltimoCambio = usuarioCreacion;
	}

	private Tramite() { }

	public static Tramite Reconstruct(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio)
	{
		if (id == Guid.Empty) throw new DominioException("Id inválido.");
		if (expedienteId == Guid.Empty) throw new DominioException("ExpedienteId inválido.");
		if (!Enum.IsDefined(typeof(EtiquetaTramite), etiqueta)) throw new DominioException("Etiqueta inválida.");
		if (contenido is null) throw new DominioException("Contenido es obligatorio.");
		if (usuarioUltimoCambio == Guid.Empty) throw new DominioException("Usuario último cambio inválido.");
		if (fechaUltimaModificacion < fechaCreacion) throw new DominioException("FechaUltimaModificacion no puede ser menor que FechaCreacion.");

		return new Tramite
		{
			Id = id,
			ExpedienteId = expedienteId,
			Etiqueta = etiqueta,
			Contenido = contenido,
			FechaCreacion = fechaCreacion,
			FechaUltimaModificacion = fechaUltimaModificacion,
			UsuarioUltimoCambio = usuarioUltimoCambio
		};
	}

	public void ActualizarContenido(ContenidoTramite nuevoContenido, Guid usuarioId)
	{
		if (nuevoContenido is null) throw new DominioException("Contenido es obligatorio.");
		if (usuarioId == Guid.Empty) throw new DominioException("Usuario inválido.");

		Contenido = nuevoContenido;
		FechaUltimaModificacion = DateTime.UtcNow;
		if (FechaUltimaModificacion < FechaCreacion) FechaUltimaModificacion = FechaCreacion;
		UsuarioUltimoCambio = usuarioId;
	}

	public void CambiarEtiqueta(EtiquetaTramite nuevaEtiqueta, Guid usuarioId)
	{
		if (!Enum.IsDefined(typeof(EtiquetaTramite), nuevaEtiqueta)) throw new DominioException("Etiqueta inválida.");
		if (usuarioId == Guid.Empty) throw new DominioException("Usuario inválido.");

		Etiqueta = nuevaEtiqueta;
		FechaUltimaModificacion = DateTime.UtcNow;
		if (FechaUltimaModificacion < FechaCreacion) FechaUltimaModificacion = FechaCreacion;
		UsuarioUltimoCambio = usuarioId;
	}
}
