using IResult = Astrum.SharedLib.Common.Results.IResult;

namespace Astrum.SharedLib.Application.Contracts.Infrastructure;

public interface IHttpClientLogger
{
    /// <summary>
    ///     Идентификатор на случай, если логи нужно привязать к чему-либо
    /// </summary>
    void SetUniqueId(Guid? guid);

    /// <summary>
    ///     Идентификатор на случай, если логи нужно привязать к чему-либо
    /// </summary>
    void SetUniqueId(long? id);

    Task<IResult> Log(HttpResponseMessage msg);
}