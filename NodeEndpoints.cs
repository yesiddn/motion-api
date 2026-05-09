using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using motion_api.Application.Nodes;
using motion_api.Presentation.Filters;

namespace motion_api;

public static class NodeEndpoints
{
    public static IEndpointRouteBuilder MapNodeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/nodes")
            .WithTags("Nodes")
            .AddEndpointFilter<AdminKeyFilter>();

        group.MapPost("/", CreateNodeAsync);

        return app;
    }

    private static async Task<Results<Created<NodeResponse>, BadRequest<ProblemDetails>, Conflict<ProblemDetails>, ProblemHttpResult>>
        CreateNodeAsync(
            CreateNodeRequest request,
            INodeService nodeService,
            CancellationToken ct)
    {
        var result = await nodeService.CreateAsync(request, ct);

        return result.Status switch
        {
            CreateNodeStatus.Created => TypedResults.Created($"/nodes/{result.Node!.MacAddress}", result.Node),
            CreateNodeStatus.DuplicateNode => TypedResults.Conflict(new ProblemDetails
            {
                Title = "Duplicate Node",
                Detail = $"A node with MAC address '{request.MacAddress}' already exists."
            }),
            _ => TypedResults.Problem("Unable to create node.", statusCode: 500)
        };
    }
}
