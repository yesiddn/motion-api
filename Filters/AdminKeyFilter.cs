namespace motion_api.Presentation.Filters;

public class AdminKeyFilter : IEndpointFilter
{
  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
  {
    var provided = ctx.HttpContext.Request.Headers["x-admin-key"].ToString();
    var expected = Environment.GetEnvironmentVariable("ADMIN_KEY");

    if (string.IsNullOrEmpty(provided) || provided != expected)
      return TypedResults.Problem("Invalid or missing admin key.", statusCode: 401);

    return await next(ctx);
  }
}