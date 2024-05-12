// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc.Routing;
// using Microsoft.AspNetCore.Routing;
// using PushCore.Infrastructure.Repository.EntityBasedRepositories;
// using PushCore.Module.Core.Models;
// using PushCore.Module.Core.SharedKernel.Specifications;
//
// namespace PushCore.Infrastructure.Extensions;
//
// public class SlugRouteValueTransformer : DynamicRouteValueTransformer
// {
//     private readonly ISluggedEntityRepository _entityRepository;
//
//     public SlugRouteValueTransformer(ISluggedEntityRepository entityRepository)
//     {
//         _entityRepository = entityRepository;
//     }
//
//     public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext,
//         RouteValueDictionary values)
//     {
//         var requestPath = httpContext.Request.Path.Value;
//
//         if (!string.IsNullOrEmpty(requestPath) && requestPath[0] == '/')
//             // Trim the leading slash
//             requestPath = requestPath.Substring(1);
//
//         var specification = new Specification<SluggedEntity>(x => x.Slug == requestPath);
//         var entity = await _entityRepository
//             .FirstOrDefaultAsync(specification);
//
//         if (entity == null) return null;
//
//         return new RouteValueDictionary
//         {
//             {"area", entity.EntityType.AreaName},
//             {"controller", entity.EntityType.RoutingController},
//             {"action", entity.EntityType.RoutingAction},
//             {"id", entity.EntityId}
//         };
//     }
// }

