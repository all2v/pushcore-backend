namespace Astrum.IdentityServer.DomainServices.ViewModels;

public class AuthOperationResult
{
    public bool Successful { get; set; }
    public string State { get; set; }
    public string SessionState { get; set; }
    public string Code { get; set; }
    public string ErrorMessage { get; set; }
}