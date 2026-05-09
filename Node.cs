namespace motion_api;

public class Node
{
  public string MacAddress { get; set; } = string.Empty;

  public string? Name { get; set; }

  public string? Location { get; set; }

  public DateTime LastSeen { get; set; }

  public ICollection<Report> Reports { get; set; } = new List<Report>();

  public ICollection<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();
}
