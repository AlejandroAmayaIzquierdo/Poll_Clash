

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WS.Models;
using WS.DB;

namespace WS.Routes.Poll;

public class PollsRoute : IRoute
{
    public string MODULE => "/polls";

    public void Register(ref WebApplication app)
    {
        var pollRoute = app.MapGroup(MODULE);


        pollRoute.MapPost("/", async (MySqliteContext context, [FromBody] PollCreation pollJson) =>
        {
            var poll = new Models.Poll
            {
                Text = pollJson.text,
                Options = pollJson.options.Select(optionText => new Option
                {
                    Text = optionText
                }).ToList()
            };

            context.Polls.Add(poll);

            await context.SaveChangesAsync();


            return Results.Ok(poll);
        });

        pollRoute.MapGet("/{id}", async (MySqliteContext context, int id) =>
        {
            var a = await context.Polls.Include(e => e.Options).FirstOrDefaultAsync(e => e.PollId == id);
            return Results.Ok(a);
        });
    }
}
