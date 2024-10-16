namespace WS.Routes;

public interface IRoute
{
    string MODULE { get; }

    public void Register(ref WebApplication app);
}