using Astrum.ITDictionary.Enums;

namespace Astrum.ITDictionary.Models.ViewModels;

/// <summary>
/// Класс для статистики
/// </summary>
public class PracticeView
{
    public PracticeType Type { get; set; }

    public int CountCompleted { get; set; }
}