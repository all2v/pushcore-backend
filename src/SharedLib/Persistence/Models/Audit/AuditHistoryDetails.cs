namespace Astrum.SharedLib.Persistence.Models.Audit;

/// <summary>
///     Changed Values details
/// </summary>
public class AutoHistoryDetails
{
    /// <summary>
    ///     The values after action.
    ///     Key contains column name and Value the value of the column.
    /// </summary>
    public Dictionary<string, object> NewValues { get; set; } = new();

    /// <summary>
    ///     The values before the action.
    ///     Key contains column name and Value the value of the column.
    /// </summary>
    public Dictionary<string, object> OldValues { get; set; } = new();
}