namespace Astrum.ITDictionary.Models.ViewModels;

public class TermView
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Definition { get; set; }

    public TermsCategoryView Category { get; set; }
}