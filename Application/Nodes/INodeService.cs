namespace motion_api.Application.Nodes;

public interface INodeService
{
    Task<CreateNodeResult> CreateAsync(CreateNodeRequest request, CancellationToken cancellationToken = default);
}
