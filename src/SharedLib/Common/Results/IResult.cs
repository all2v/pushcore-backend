using System.Text.Json.Serialization;

namespace Astrum.SharedLib.Common.Results
{
    public interface IResult
    {
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
        List<ValidationError> ValidationErrors { get; }
        [JsonIgnore]
        Type ValueType { get; }
        object GetValue();
    }
}
