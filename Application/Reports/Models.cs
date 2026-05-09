namespace motion_api.Application.Reports;

public sealed record CreateReportRequest(string NodeId, string EventType, DateTime? Timestamp);

public sealed record ReportResponse(Guid Id, string NodeId, DateTime Timestamp, string EventType);

public enum CreateReportStatus
{
  Created,
  InvalidEventType,
  NodeNotFound
}

public sealed record CreateReportResult(CreateReportStatus Status, ReportResponse? Report = null);