namespace Astrum.SharedLib.Domain.Interfaces;

public interface IEntity
{
    
}
/// <summary>
///     Generic abstraction for a domain entity
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IEntity<out TId> : IEntity
{
    /// <summary>
    ///     Entity unique identifier
    /// </summary>
    TId Id { get; }
}