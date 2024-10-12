
using Microsoft.EntityFrameworkCore;
using WS.Models;

namespace WS.DB;
public class MySqliteContext(DbContextOptions options) : DbContext(options)
{
    // DbSet for Polls (matches your Poll model)
    public DbSet<Poll> Polls { get; set; }

    // DbSet for Options (matches your Option model)
    public DbSet<Option> Options { get; set; }


}