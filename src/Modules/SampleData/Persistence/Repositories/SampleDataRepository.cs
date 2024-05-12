using Astrum.SampleData.Persistence;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.SampleData.Repositories;

public class SampleDataRepository :ISampleDataRepository
{
    private readonly BaseDbContext _dbContext;

    public SampleDataRepository(SampleDataDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void RunCommand(string command)
    {
        //_logger.LogDebug(command);
        try
        {
            var res = _dbContext.Database.ExecuteSqlRaw(command);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void RunCommands(IEnumerable<string> commands)
    {
        foreach (var command in commands)
        {
            RunCommand(command);
        }
    }

    public IEnumerable<string> PostgresCommands(IEnumerable<string> lines)
    {
        var commands = new List<string>();
        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                commands.Add(line);
            }
        }

        return commands;
    }

    public string GetDbConnectionType()
    {
        var dbConntionType = _dbContext.Database.GetDbConnection().GetType();
        return dbConntionType.ToString();
    }
 }
