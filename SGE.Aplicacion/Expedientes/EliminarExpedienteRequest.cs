using System;

namespace SGE.Aplicacion.Expedientes;

public record class EliminarExpedienteRequest
{
    public Guid Id { get; set; }
    public Guid UsuarioUltimoCambio { get; set; }
}
