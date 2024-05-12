using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Astrum.Core.Common.Results.V1;

public interface IResult
{
    /// <summary>
    ///     Код статуса операции
    /// </summary>
    [Required]
    [System.Text.Json.Serialization.JsonIgnore]
    int StatusCode { get; }

    /// <summary>
    ///     Флаг успешности операции
    /// </summary>
    bool Succeeded { get; }

    /// <summary>
    ///     Флаг провала операции
    /// </summary>
    bool Failed => !Succeeded;

    [System.Text.Json.Serialization.JsonIgnore]
    Exception Exception { get; set; }

    string Message { get; }

    /// <summary>
    ///     Описание возникшей ошибки
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    string MessageWithErrors { get; }

    IReadOnlyCollection<IResultError> Errors { get; }
    void AddError(IResultError error);
}

public interface IResult<out T> : IResult
{
    T Data { get; }
}