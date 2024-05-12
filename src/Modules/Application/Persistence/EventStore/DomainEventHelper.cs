using System;
using Astrum.SharedLib.Domain.Interfaces;
using Newtonsoft.Json;

namespace Astrum.Application.EventStore;

public static class DomainEventHelper
{
    private static readonly JsonSerializerSettings _jsonSerializerSettings =
        new() {ContractResolver = new PrivateSetterContractResolver()};

    public static IDomainEvent<TAggregateId> ConstructDomainEvent<TAggregateId>(string data, string assemblyTypeName)
    {
        var type = Type.GetType(assemblyTypeName);
        var domainEvent =
            (IDomainEvent<TAggregateId>)JsonConvert.DeserializeObject(data, type, _jsonSerializerSettings);
        return domainEvent;
    }
}