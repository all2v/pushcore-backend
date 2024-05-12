using Newtonsoft.Json;

namespace Astrum.Core.Common.Results;

public interface IResultError : IResult
{
    /// <summary>
    ///     Флаг успешности операции
    /// </summary>
    /// <example>false</example>
    /// <remarks>Для документации сваггера, чтобы в ней был false в ошибках</remarks>
    public new bool Succeeded { get; set; }


    /// <summary>
    ///     Описание возникшей ошибки
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ExceptionMessage { get; }

    /// <summary>
    ///     Описание возникшей ошибки для отображения пользователю
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ClientReadableExceptionMessage { get; set; }
}

public interface IError
{
    string Type { get; }
    IDictionary<string, object> Data { get; }
}