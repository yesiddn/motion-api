namespace motion_api.Application.Reports;

public interface IReportService
{
  Task<CreateReportResult> CreateAsync(CreateReportRequest request, CancellationToken cancellationToken = default);
}
