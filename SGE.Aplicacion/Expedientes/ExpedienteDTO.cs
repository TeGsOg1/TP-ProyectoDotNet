using System;

namespace SGE.Aplicacion.Expedientes;

public record class ExpedienteDTO
{
    public Guid Id { get; set; }
    public string Caratula { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaUltimaModificacion { get; set; }
    public Guid UsuarioUltimoCambio { get; set; }
    public string? Estado { get; set; }
}
