using System.ComponentModel.DataAnnotations;

namespace motion_api.Application.Nodes;

public sealed record CreateNodeRequest(
    [Required(ErrorMessage = "MacAddress is required.")]
    [RegularExpression(@"^([0-9A-Fa-f]{2}:){5}[0-9A-Fa-f]{2}$", ErrorMessage = "MAC address must be in the format XX:XX:XX:XX:XX:XX.")]
    string MacAddress,

    [Required(ErrorMessage = "Name is required.")]
    string Name,

    [Required(ErrorMessage = "Location is required.")]
    string Location
);

public sealed record NodeResponse(string MacAddress, string? Name, string? Location, DateTime LastSeen);

public enum CreateNodeStatus
{
    Created,
    InvalidMacAddress,
    DuplicateNode
}

public sealed record CreateNodeResult(CreateNodeStatus Status, NodeResponse? Node = null);
