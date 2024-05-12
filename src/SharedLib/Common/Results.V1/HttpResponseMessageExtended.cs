namespace Astrum.Core.Common.Results;

public class HttpResponseMessageExtended
{
    public HttpResponseMessageExtended(HttpResponseMessage httpResponseMessage)
    {
        HttpResponseMessage = httpResponseMessage;
        IsSuccess = httpResponseMessage.IsSuccessStatusCode;
    }

    public HttpResponseMessageExtended(Exception exception)
    {
        Exception = exception;
        IsSuccess = false;
    }

    public bool IsSuccess { get; set; }

    public Exception Exception { get; set; }

    public HttpResponseMessage HttpResponseMessage { get; set; }
}