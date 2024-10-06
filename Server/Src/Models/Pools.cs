

namespace WS.Models;

public class Pool
{
    public required string Text { get; set; }

    public required Option[] Options { get; set; }

    public Pool()
    {
        // if (Options == null || Options.Length == 0)
        //     throw new Exception("It can generated a pool without options");
    }


    public Option? GetWinnableOption()
    {
        return Options.FirstOrDefault(option => option.Votes == Options.Max(o => o.Votes));
    }

}

public class Option
{
    public required int Id { get; set; }
    public required string Text { get; set; }
    public int Votes { get; set; } = 0;

}