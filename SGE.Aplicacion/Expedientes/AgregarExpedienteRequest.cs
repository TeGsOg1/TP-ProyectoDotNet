using System;

namespace SGE.Aplicacion.Expedientes;

public record class AgregarExpedienteRequest
{
    public string Caratula { get; set; }
    public Guid idUsuario { get; set; }

}
