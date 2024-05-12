using Astrum.Appeal.ViewModels;
using HotChocolate;
using HotChocolate.Types;

namespace Astrum.Appeal.GraphQL;

public class SubscriptionAppeal
{
    [Subscribe]
    public IEnumerable<AppealForm> AppealsChanged([EventMessage] IEnumerable<AppealForm> appeals)
    {
        return appeals;
    }
}