namespace Astrum.CodeRev.Domain.Aggregates.Draft;

public class Draft
{
    public string Text { get; set; }
    public Checkbox[] Checkboxes { get; set; }
}