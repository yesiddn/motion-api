using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace motion_api.Application.Nodes;

public sealed class NodeService : INodeService
{
    private readonly AppDbContext _dbContext;

    public NodeService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateNodeResult> CreateAsync(CreateNodeRequest request, CancellationToken ct = default)
    {
        var node = new Node
        {
            MacAddress = request.MacAddress,
            Name = request.Name,
            Location = request.Location,
            LastSeen = DateTime.UtcNow
        };

        try
        {
            _dbContext.Nodes.Add(node);
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            return new CreateNodeResult(CreateNodeStatus.DuplicateNode);
        }

        var response = new NodeResponse(node.MacAddress, node.Name, node.Location, node.LastSeen);
        return new CreateNodeResult(CreateNodeStatus.Created, response);
    }
}
