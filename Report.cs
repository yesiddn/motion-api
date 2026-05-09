namespace motion_api;

public class Report
{
  public Guid Id { get; set; }

  public string NodeId { get; set; } = string.Empty;

  public DateTime Timestamp { get; set; }

  public EventType EventType { get; set; }

  public Node? Node { get; set; }
}
