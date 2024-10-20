

using lib;

namespace WS.Models;

public class VoteEventData : BaseDto
{
    public required string id { get; set; }
    public required int option { get; set; }
}

public class ConnectEventData : BaseDto
{
    public required string id { get; set; }
}