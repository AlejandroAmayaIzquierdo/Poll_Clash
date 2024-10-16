

namespace WS.Models;

public class Poll
{
    public int PollId { get; set; }

    public required string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Creation timestamp

    // Navigation property: A poll has many options
    public required List<Option> Options { get; set; } = [];

    // Business logic to get the option with the maximum votes
    public Option? GetWinnableOption()
    {
        return Options.FirstOrDefault(option => option.Votes == Options.Max(o => o.Votes));
    }
}

public class Option
{
    public int OptionId { get; set; } // Primary key for Options

    public required string Text { get; set; }

    public int Votes { get; set; } = 0;

    // Foreign key to the associated Poll
    public int PollId { get; set; }
}

public record PollCreation(string text, List<string> options);