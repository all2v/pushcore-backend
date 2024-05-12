using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Articles.Aggregates;

public class Author : AggregateRootBase<Guid>
{
    //TODO: remove author entity, use ApplicationUser instead
    private Author()
    {
        
    }
    public Guid UserId { get; set; }

    public Author(Guid userId)
    {
        UserId = userId;
    }
    
    
}