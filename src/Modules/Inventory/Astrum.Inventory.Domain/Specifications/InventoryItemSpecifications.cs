using System.Linq;
using Ardalis.Specification;
using Astrum.Inventory.Domain.Aggregates;

namespace Astrum.Inventory.Domain.Specifications
{
    public class GetInventoryItemsSpec : Specification<InventoryItem>
    {
        public GetInventoryItemsSpec()
        {
            Query
                .Include(c => c.Template);
        }
    }
    public class GetInventoryItemById : GetInventoryItemsSpec
    {
        public GetInventoryItemById(Guid id)
        {
            Query
                .Where(c => c.Id == id);
        }
    }
    public class GetInventoryItemsByUserId : GetInventoryItemsSpec
    {
        public GetInventoryItemsByUserId(Guid userId)
        {
            Query
                .Where(c => c.CreatedBy == userId.ToString());
        }
    }
    public class GetFilteringInventoryItems : GetInventoryItemsSpec
    {
        public GetFilteringInventoryItems(string[]? templates, string? predicate, Status[]? statuses, Guid? userId,
            int? startIndex = 1, int? count = 15)
        {
            Query
                .Where(c => userId == null || c.UserId == userId)
                .Where(c => statuses.Count() == 0 || statuses.Contains(c.Status))
                .Where(c => templates.Count() == 0 || templates.Contains(c.Template.Title))
                .Where(c => predicate == null || c.Model.ToLower().Contains(predicate.ToLower()))
                .OrderBy(e => e.Index)
                .Where(e => e.Index > startIndex)
                .Take(count.Value);
        }
    }
}

public class CheckNextSpec : Specification<InventoryItem>
{
    public CheckNextSpec(int index)
    {
        Query
            .OrderBy(e => e.Index)
            .Where(e => e.Index > index);
    }
}