namespace Astrum.Core.Common.Results;

/// <summary>
///     Generic result wrapper. See <see cref="Result" />, see <see cref="IResult{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
public class Result<T> : Result, IResult<T>
{
    #region IResult<T> Members

    public T Data { get; set; }

    #endregion
}