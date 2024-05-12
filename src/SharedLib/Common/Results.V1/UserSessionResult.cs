namespace Astrum.Core.Common.Results;

public class UserSessionResult : Result
{
    /// <summary>
    ///     Guid пользовательской сессии. Необходим для логирования
    /// </summary>
    public Guid Data { get; set; }
}