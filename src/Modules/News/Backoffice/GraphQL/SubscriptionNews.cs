using Astrum.News.ViewModels;
using HotChocolate;
using HotChocolate.Types;

namespace Astrum.News.GraphQL;

public class SubscriptionNews
{
    [Subscribe]
    public NewsForm NewsChanged([EventMessage] NewsForm news)
    {
        return news;
    }
}