using Newtonsoft.Json;

namespace Astrum.Core.Common.Results;

public class ResultError : Result, IResultError
{
    public ResultError()
    {
        Succeeded = false;
    }

    public ResultError(string error, string clientReadableErrorMessage = null)
    {
        ExceptionMessage = error;
        ClientReadableExceptionMessage = clientReadableErrorMessage;
    }

    public ResultError(string error, int code, string clientReadableExceptionMessage = null)
        : this(error, clientReadableExceptionMessage)
    {
        StatusCode = code;
    }

    #region IResultError Members

    public new bool Succeeded { get; set; }

    /// <summary>
    ///     Описание возникшей ошибки
    /// </summary>
    public string ExceptionMessage { get; }

    /// <summary>
    ///     Описание возникшей ошибки для отображения пользователю
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ClientReadableExceptionMessage { get; set; }

    #endregion

    public override string ToString()
    {
        return $"Error[{StatusCode}]: {ExceptionMessage}";
    }
}