namespace motion_api.Application.ApiKeys;

public sealed record CreateApiKeyRequest(string NodeId);

public sealed record CreateApiKeyResponse(string Key, string NodeId);

public enum CreateApiKeyStatus
{
    Created,
    NodeNotFound
}

public sealed record CreateApiKeyResult(CreateApiKeyStatus Status, CreateApiKeyResponse? ApiKey = null);

public sealed record RevokeApiKeyRequest(string Key);

public enum RevokeApiKeyStatus
{
    Revoked,
    KeyNotFound,
    AlreadyRevoked
}

public sealed record RevokeApiKeyResult(RevokeApiKeyStatus Status);
