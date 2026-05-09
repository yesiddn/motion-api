using Microsoft.EntityFrameworkCore;

namespace motion_api.Application.Reports;

public sealed class ReportService : IReportService
{
  private readonly AppDbContext _dbContext;

  public ReportService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<CreateReportResult> CreateAsync(CreateReportRequest request, CancellationToken ct = default)
  {
    // Validación rápida en memoria antes de golpear la base de datos
    if (!Enum.TryParse<EventType>(request.EventType, ignoreCase: true, out var eventType))
    {
      return new CreateReportResult(CreateReportStatus.InvalidEventType);
    }

    // Buscar el nodo
    var node = await _dbContext.Nodes.FirstOrDefaultAsync(n => n.MacAddress == request.NodeId, ct);

    if (node is null)
    {
      return new CreateReportResult(CreateReportStatus.NodeNotFound);
    }

    // Lógica de negocio
    var timestamp = request.Timestamp ?? DateTime.UtcNow;

    var report = new Report
    {
      Id = Guid.NewGuid(),
      NodeId = request.NodeId,
      Timestamp = timestamp,
      EventType = eventType
    };

    node.LastSeen = timestamp;

    _dbContext.Reports.Add(report);

    // Guardar ambos cambios (Insertar Reporte + Actualizar LastSeen) en una sola transacción
    await _dbContext.SaveChangesAsync(ct);

    var response = new ReportResponse(report.Id, report.NodeId, report.Timestamp, report.EventType.ToString());

    return new CreateReportResult(CreateReportStatus.Created, response);
  }
}