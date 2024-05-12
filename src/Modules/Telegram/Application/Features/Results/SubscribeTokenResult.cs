namespace Astrum.Telegram.Application.Featues.Result;

public record SubscribeTokenResult
{
    public string Token { get; set; }
    public string Link { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool Success { get; set; }
    public string Error { get; set; }
}