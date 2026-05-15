using System;
using System.Collections.Generic;

namespace SGE.Aplicacion.Expedientes;

public interface IExpedienteRepository
{
    void AgregarExpediente(Expediente expediente);
    void EliminarExpediente(Guid id);
    void ActualizarExpediente(Expediente expediente);
    Expediente? ObtenerExpedientePorId(Guid id);
    IEnumerable<Expediente>ObtenerTodosExpedientes();
}
