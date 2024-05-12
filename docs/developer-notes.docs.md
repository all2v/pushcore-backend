# ASTRUM - Guide for developers

##   

- Classes inheriting from DomainEventBase<TId> need to have an internal empty constructor

- Classes inheriting from AggregateRootBase<TId> meant for use with the event store, need to have a private empty
  constructor

- Commands and queries should implement IRequest and IHandler respectively

- Data-only (persistence-related) models should implement IDataEntity<TId>

- Aggregate roots should implement IAggregateRoot<T, TId>
