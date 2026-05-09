namespace motion_api.Application.ApiKeys;

public interface IApiKeyService
{
    Task<CreateApiKeyResult> CreateAsync(CreateApiKeyRequest request, CancellationToken cancellationToken = default);
    Task<RevokeApiKeyResult> RevokeAsync(RevokeApiKeyRequest request, CancellationToken cancellationToken = default);
}
