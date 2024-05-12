using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.ITDictionary.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Astrum.ITDictionary;

public class DbContextInitializer : IDbContextInitializer
{
    public DbContextInitializer(ITDictionaryDbContext context)
    {
        _context = context;
    }

    private ITDictionaryDbContext _context { get; set; }

    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        await _context.Database.MigrateAsync(cancellationToken);
    }

    public Task Seed(CancellationToken cancellationToken = default)
    {
        if (_context.Terms.Any() || _context.Categories.Any())
            return Task.CompletedTask;

        var categories = new Category[]
        {
            new Category { Id = new Guid("902bed00-e90d-4b3e-9d92-43a6c0dc5e0c"), Name = "Языки программирования" },
            new Category { Id = new Guid("c2756b77-eef3-4f19-94ef-8008ab46619d"), Name = "Базы данных" },
        };

        var terms = new Term[]
        {
            new Term
            {
                Id = new Guid("5b944bd0-907e-432a-941b-30455c4b0759"),
                Name = "Python",
                Definition =
                    "Универсальный и удобный для начинающих язык программирования, известный своей простотой и читабельностью.",
                Category = categories[0],
            },
            new Term
            {
                Id = new Guid("5ff82078-d36b-4310-a36f-12639eb492c2"),
                Name = "Java",
                Definition =
                    "Популярный объектно-ориентированный язык программирования, используемый для создания приложений корпоративного уровня и приложений для Android.",
                Category = categories[0],
            },
            new Term
            {
                Id = new Guid("4df22989-c575-452f-9324-e02a0592ccdc"),
                Name = "Запрос",
                Definition =
                    "Запрос данных или информации из базы данных, обычно выраженный на структурированном языке запросов (SQL).",
                Category = categories[1],
            },
            new Term
            {
                Id = new Guid("5dc51671-0bf7-4cb4-9fb4-562bed8e0b51"),
                Name = "Реляционная база данных",
                Definition =
                    "Тип базы данных, который организует данные в таблицы и обеспечивает взаимосвязи между таблицами.",
                Category = categories[1],
            },
        };

        _context.Terms.AddRange(terms);
        _context.SaveChanges();
        return Task.CompletedTask;
    }
}