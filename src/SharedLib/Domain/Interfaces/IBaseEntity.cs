namespace Astrum.SharedLib.Domain.Interfaces;

public interface IBaseEntity<out TId> : IEntity<TId>, ISafetyRemovableEntity
{
}