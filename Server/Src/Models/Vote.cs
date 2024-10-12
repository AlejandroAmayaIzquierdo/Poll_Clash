

using lib;

namespace WS.Vote;

public class VoteEventData : BaseDto
{
    public required Guid id { get; set; }
    public required int option { get; set; }
}