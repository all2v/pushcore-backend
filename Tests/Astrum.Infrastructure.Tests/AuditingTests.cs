using Astrum.Ordering.Aggregates;
using Astrum.Tests;
using Xunit;

namespace Astrum.Infrastructure.Tests;

public class AuditingTests : TestBase
{
    [Fact]
    public void Auditing_OnAdd_AuditableEntity_CreatesAuditHistoryRecord()
    {
        var order = new Order("123");
        OrderingContext.Orders.Add(order);
        ApplicationContext.SaveChanges();

        var auditHistory = ApplicationContext.AuditHistory.SingleOrDefault();

        Assert.NotNull(auditHistory);
    }
}