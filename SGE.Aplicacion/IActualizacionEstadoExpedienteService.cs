using System;

namespace SGE.Aplicacion.Expedientes;

public interface IActualizacionEstadoExpedienteService
{
    void Actualizar(Guid expedienteId, Guid idUsuario);
}
