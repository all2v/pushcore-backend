using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Articles.Aggregates
{
    public class Category : AggregateRootBase<Guid>
    {
        public Category() { }
        public Category(Guid id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public List<Tag> Tags { get; set;}
    }
}
