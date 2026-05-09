using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace motion_api.Presentation.Filters;

public class ApiKeyFilter : IEndpointFilter
{
  private readonly AppDbContext _db;

  public ApiKeyFilter(AppDbContext db) => _db = db;

  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
  {
    var provided = ctx.HttpContext.Request.Headers["x-api-key"].ToString();

    if (string.IsNullOrEmpty(provided))
      return TypedResults.Problem("Missing API key.", statusCode: 401);

    // Comparar contra el hash guardado en BD
    var hashedProvided = Convert.ToBase64String(
        SHA256.HashData(Encoding.UTF8.GetBytes(provided)));

    var key = await _db.ApiKeys
        .FirstOrDefaultAsync(k => k.Key == hashedProvided && !k.IsRevoked);

    if (key is null)
      return TypedResults.Problem("Invalid or revoked API key.", statusCode: 401);

    return await next(ctx);
  }
}