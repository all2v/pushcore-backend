using Api.Application.Authorization.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Api.Application.Authorization;

public class AuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;

    public AuthorizationBehavior(IIdentityService identityService,
        ILogger<AuthorizationBehavior<TRequest, TResponse>> logger)
    {
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region IPipelineBehavior<TRequest,TResponse> Members

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // TODO: consider reflection performance impact
        var authorizeAttributes = request
            .GetType()
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .ToList();

        if (!authorizeAttributes.Any()) return await next();

        EnsureAuthorizedRoles(authorizeAttributes);

        await EnsureAuthorizedPolicies(request, authorizeAttributes);

        // User is authorized / authorization not required
        return await next();
    }

    #endregion

    private async Task EnsureAuthorizedPolicies(
        TRequest request,
        IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        // Policy-based authorization
        var authorizeAttributesWithPolicies = authorizeAttributes
            .Where(a => !string.IsNullOrWhiteSpace(a.Policy))
            .ToList();

        if (!authorizeAttributesWithPolicies.Any()) return;

        var requiredPolicies = authorizeAttributesWithPolicies
            .Select(a =>
            {
                if (a is AuthorizeProtectedResourceAttribute resourceAttribute
                    && request is IRequestWithResourceId requestWithResourceId)
                    resourceAttribute.ResourceId = requestWithResourceId.ResourceId;

                return a.Policy;
            });

        foreach (var policy in requiredPolicies)
        {
            var authorized = await _identityService
                .AuthorizeAsync(_identityService.Principal!, policy!);

            if (authorized) continue;

            _logger.LogDebug("Failed policy authorization {Policy}", policy);
            throw new ForbiddenAccessException();
        }
    }

    private void EnsureAuthorizedRoles(IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        // Role-based authorization
        var authorizeAttributesWithRoles = authorizeAttributes
            .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
            .ToList();

        if (!authorizeAttributesWithRoles.Any()) return;

        var requiredRoles = authorizeAttributesWithRoles
            .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
            .Select(a => a.Roles!.Split(','));

        if (requiredRoles
            .Select(roles => roles.Any(
                role => _identityService.IsInRoleAsync(role.Trim())))
            .Any(authorized => !authorized))
        {
            _logger.LogDebug("Failed role authorization");
            throw new ForbiddenAccessException();
        }
    }
}