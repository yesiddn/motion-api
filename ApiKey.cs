namespace motion_api;

public class ApiKey
{
  public string Key { get; set; } = string.Empty;

  public string NodeId { get; set; } = string.Empty;

  public bool IsRevoked { get; set; }

  public Node? Node { get; set; }
}
