namespace Astrum.Telegram.Application.Featues.Result;

public record SubscribeResult
{
    public string Message { get; }
    public bool Success { get; }

    public SubscribeResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}