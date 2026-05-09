using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace motion_api.Application.ApiKeys;

public sealed class ApiKeyService : IApiKeyService
{
    private readonly AppDbContext _dbContext;

    public ApiKeyService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateApiKeyResult> CreateAsync(CreateApiKeyRequest request, CancellationToken ct = default)
    {
        // Check node exists
        var node = await _dbContext.Nodes.FirstOrDefaultAsync(n => n.MacAddress == request.NodeId, ct);
        if (node is null)
        {
            return new CreateApiKeyResult(CreateApiKeyStatus.NodeNotFound);
        }

        // Generate random key
        var plainKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        // Hash the key for storage
        var hashedKey = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(plainKey)));

        var apiKey = new ApiKey
        {
            Key = hashedKey,
            NodeId = request.NodeId,
            IsRevoked = false
        };

        _dbContext.ApiKeys.Add(apiKey);
        await _dbContext.SaveChangesAsync(ct);

        var response = new CreateApiKeyResponse(plainKey, request.NodeId);
        return new CreateApiKeyResult(CreateApiKeyStatus.Created, response);
    }

    public async Task<RevokeApiKeyResult> RevokeAsync(RevokeApiKeyRequest request, CancellationToken ct = default)
    {
        // Hash the provided key to look it up
        var hashedKey = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(request.Key)));

        var apiKey = await _dbContext.ApiKeys.FirstOrDefaultAsync(k => k.Key == hashedKey, ct);

        if (apiKey is null)
        {
            return new RevokeApiKeyResult(RevokeApiKeyStatus.KeyNotFound);
        }

        if (apiKey.IsRevoked)
        {
            return new RevokeApiKeyResult(RevokeApiKeyStatus.AlreadyRevoked);
        }

        apiKey.IsRevoked = true;
        await _dbContext.SaveChangesAsync(ct);

        return new RevokeApiKeyResult(RevokeApiKeyStatus.Revoked);
    }
}
