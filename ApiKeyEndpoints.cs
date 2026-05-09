using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using motion_api.Application.ApiKeys;
using motion_api.Presentation.Filters;

namespace motion_api;

public static class ApiKeyEndpoints
{
    public static IEndpointRouteBuilder MapApiKeyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api-keys")
            .WithTags("API Keys")
            .AddEndpointFilter<AdminKeyFilter>();

        group.MapPost("/", CreateApiKeyAsync);
        group.MapPost("/revoke", RevokeApiKeyAsync);

        return app;
    }

    private static async Task<Results<Created<CreateApiKeyResponse>, NotFound<ProblemDetails>, ProblemHttpResult>>
        CreateApiKeyAsync(
            CreateApiKeyRequest request,
            IApiKeyService apiKeyService,
            CancellationToken ct)
    {
        var result = await apiKeyService.CreateAsync(request, ct);

        return result.Status switch
        {
            CreateApiKeyStatus.Created => TypedResults.Created($"/api-keys", result.ApiKey!),
            CreateApiKeyStatus.NodeNotFound => TypedResults.NotFound(new ProblemDetails
            {
                Title = "Node Not Found",
                Detail = $"Node '{request.NodeId}' was not found in the system."
            }),
            _ => TypedResults.Problem("Unable to create API key.", statusCode: 500)
        };
    }

    private static async Task<Results<Ok, NotFound<ProblemDetails>, Conflict<ProblemDetails>, ProblemHttpResult>>
        RevokeApiKeyAsync(
            RevokeApiKeyRequest request,
            IApiKeyService apiKeyService,
            CancellationToken ct)
    {
        var result = await apiKeyService.RevokeAsync(request, ct);

        return result.Status switch
        {
            RevokeApiKeyStatus.Revoked => TypedResults.Ok(),
            RevokeApiKeyStatus.KeyNotFound => TypedResults.NotFound(new ProblemDetails
            {
                Title = "API Key Not Found",
                Detail = "The provided API key was not found in the system."
            }),
            RevokeApiKeyStatus.AlreadyRevoked => TypedResults.Conflict(new ProblemDetails
            {
                Title = "Already Revoked",
                Detail = "The provided API key has already been revoked."
            }),
            _ => TypedResults.Problem("Unable to revoke API key.", statusCode: 500)
        };
    }
}
