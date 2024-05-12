namespace Astrum.SampleData.Repositories;

public interface ISampleDataRepository
{
    void RunCommand(string command);

    void RunCommands(IEnumerable<string> command);

    IEnumerable<string> PostgresCommands(IEnumerable<string> lines);

    string GetDbConnectionType();
}
