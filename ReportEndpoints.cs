
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using motion_api.Application.Reports;
using motion_api.Presentation.Filters;

namespace motion_api;

public static class ReportEndpoints
{
  // Método de extensión sobre IEndpointRouteBuilder
  public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
  {
    // Agrupas todos los endpoints de reportes aquí (puedes agregar filtros, auth, etc.)
    var group = app.MapGroup("/reports")
        .WithTags("Reports")
        .AddEndpointFilter<ApiKeyFilter>(); // <-- Aquí va el filtro de nodo

    group.MapPost("/", CreateReportAsync);

    return app;
  }

  // Separar el handler hace que el código sea aún más limpio
  private static async Task<Results<Created<ReportResponse>, BadRequest<ProblemDetails>, NotFound<ProblemDetails>, ProblemHttpResult>>
      CreateReportAsync(
          CreateReportRequest request,
          IReportService reportService,
          CancellationToken ct)
  {
    var result = await reportService.CreateAsync(request, ct);

    return result.Status switch
    {
      CreateReportStatus.Created => TypedResults.Created($"/reports/{result.Report!.Id}", result.Report),

      CreateReportStatus.InvalidEventType => TypedResults.BadRequest(new ProblemDetails
      {
        Title = "Invalid Event Type",
        Detail = "EventType must be 'Heartbeat' or 'Motion'."
      }),

      CreateReportStatus.NodeNotFound => TypedResults.NotFound(new ProblemDetails
      {
        Title = "Node Not Found",
        Detail = $"Node '{request.NodeId}' was not found in the system."
      }),

      _ => TypedResults.Problem("Unable to create report.", statusCode: 500)
    };
  }
}