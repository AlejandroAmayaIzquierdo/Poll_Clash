

using lib;

namespace WS.Vote;

public class VoteEventData : BaseDto
{
    public required int option { get; set; }
}