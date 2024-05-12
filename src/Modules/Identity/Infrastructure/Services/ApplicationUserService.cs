using System.Linq.Dynamic.Core;
using System.Transactions;
using Astrum.Identity.Contracts;
using Astrum.Identity.DTOs;
using Astrum.Identity.Models;
using Astrum.SharedLib.Common.Options;
using Astrum.SharedLib.Common.Results;
using Astrum.SharedLib.Domain.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Astrum.Identity.Services;

/// <inheritdoc cref="IApplicationUserService" />
internal class ApplicationUserService : IApplicationUserService
{
    // private readonly ILocalizationService _localizer;
    private readonly ILogger<ApplicationUserService> _logger;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserService(SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, ILogger<ApplicationUserService> logger, IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
    }

    #region IApplicationUserService Members

    public async Task<Result> ActivateUser(string username)
    {
        try
        {
            var applicationUser = await _userManager.FindByNameAsync(username);

            var validationResult = ValidateUserForActivation(applicationUser);
            if (!validationResult.IsSuccess)
                return validationResult;

            applicationUser.IsActive = true;
            var result = await _userManager.UpdateAsync(applicationUser);

            if (result.Succeeded)
                return Result.Success();

            return Result.Error("Unable to activate user.");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to activate user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result> DeactivateUser(string username)
    {
        try
        {
            var applicationUser = await _userManager.FindByNameAsync(username);

            if (applicationUser == null)
                return Result.Error("User not found.");

            if (!applicationUser.IsActive)
                return Result.Error("User is not active.");

            applicationUser.IsActive = false;
            var result = await _userManager.UpdateAsync(applicationUser);

            if (result.Succeeded)
                return Result.Success(_mapper.Map<Result>(result));
            return Result.Error("Unable to deactivate user.");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to deactivate user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result> AddRoles(RoleAssignmentRequestDto request)
    {
        try
        {
            var applicationUser = await _userManager.FindByNameAsync(request.Username);

            if (applicationUser == null)
                return Result.Error("User not found.");

            var validationResult = await ValidateRolesForAdditionAsync(request.Roles, applicationUser);
            if (!validationResult.IsSuccess)
                return validationResult;

            var result = await _userManager.AddToRolesAsync(applicationUser, request.Roles.Select(nr => nr.ToUpper()));

            if (result.Succeeded)
                return Result.Success(_mapper.Map<Result>(result));
            return Result.Error("Unable to add role user.");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to add role to user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result> RemoveRoles(RoleAssignmentRequestDto request)
    {
        try
        {
            var applicationUser = await _userManager.FindByNameAsync(request.Username);
            if (applicationUser == null)
                return Result.Error("User not found.");

            var validationResult = await ValidateRolesForRemovalAsync(request.Roles, applicationUser);
            if (!validationResult.IsSuccess)
                return validationResult;

            var result =
                await _userManager.RemoveFromRolesAsync(applicationUser, request.Roles.Select(r => r.ToUpper()));

            if (result.Succeeded)
                return Result.Success(_mapper.Map<Result>(result));
            return Result.Error("Unable to remove role from user.");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to remove role from user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result> CreateUser(ApplicationUser user, string password, List<string>? roles,
        bool isActive = false)
    {
        Result? serviceResult;
        try
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            applicationUser.Id = Guid.NewGuid();
            applicationUser.IsActive = isActive;
            applicationUser.MustChangePassword = true;
            var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using (transaction)
            {
                var identityResult = await _userManager.CreateAsync(applicationUser, password);

                if (!identityResult.Succeeded)
                    serviceResult = _mapper.Map<Result>(identityResult);
                else if (roles?.Any() ?? false)
                {
                    var rolesResult =
                        await _userManager.AddToRolesAsync(applicationUser, roles.Select(nr => nr.ToUpper()));
                    serviceResult = _mapper.Map<Result>(rolesResult);
                }
                else
                    serviceResult = Result.Success();

                transaction.Complete();
            }
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to create user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }

        return serviceResult;
    }

    public async Task<Result> ChangePassword(string username, string password)
    {
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        try
        {
            var applicationUser = await _userManager.FindByNameAsync(username);
            if (applicationUser == null)
                return Result.Error("Пользователь не найден.");

            //var hashResult =
            //    passwordHasher.VerifyHashedPassword(applicationUser, applicationUser.PasswordHash, password);
            //if (hashResult == PasswordVerificationResult.Failed)
            //{
            //    return Result.Invalid(new List<ValidationError>() { new ValidationError { ErrorMessage = "Ошибка валидации при смене пароля.", ErrorCode = "401", Identifier = "0" } });
            //}
            var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
            var result = await _userManager.ResetPasswordAsync(applicationUser, token, password);

            if (result.Succeeded)
            {
                var user = await _userManager.UpdateAsync(applicationUser);

                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(applicationUser, password, false, false);
                return Result.Success();
            }

            return Result.Error("Не удалось изменить пароль.");
        }
        catch (Exception ex)
        {
            var errorMessage = "Ошибка во-время смены пароль.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(ex.Message, errorMessage);
        }
    }

    public async Task<Result<ApplicationUser>> GetUserAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await GetUsersWithRoles().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            return Result.Success(data);
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to get the user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<ApplicationUser>> GetUserByNameAsync(string username,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await GetUsersWithRoles().FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
            return Result.Success(data);
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to get the user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result> UpdateRoles(string username, List<string> roles)
    {
        try
        {
            var applicationUser = await _userManager.FindByNameAsync(username);
            if (applicationUser == null)
                return Result.Error("User not found.");
            var existingRoles = await _userManager.GetRolesAsync(applicationUser);
            var identityResult = await _userManager.RemoveFromRolesAsync(applicationUser, existingRoles);

            if (!identityResult.Succeeded)
                return Result.Error("Unable to update user roles.");

            identityResult = await _userManager.AddToRolesAsync(applicationUser, roles.Select(r => r.ToUpper()));

            if (identityResult.Succeeded)
                return Result.Success(_mapper.Map<Result>(identityResult));
            return Result.Error("Unable to update user roles.");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to update roles of user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result> UpdateUserDetails(UpdateUserDetailsDto request)
    {
        try
        {
            var applicationUser = await _userManager.FindByIdAsync(request.Id);

            if (applicationUser == null)
                return Result.Error("User not found.");

            if (!string.IsNullOrWhiteSpace(request.Email) && applicationUser.Email != request.Email)
                applicationUser.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Name) && applicationUser.Name != request.Name)
                applicationUser.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Username) && applicationUser.UserName != request.Username)
                applicationUser.UserName = request.Username;

            if (!string.IsNullOrWhiteSpace(request.PrimaryPhone) &&
                applicationUser.PhoneNumber != request.PrimaryPhone)
                applicationUser.PhoneNumber = request.PrimaryPhone;

            if (!string.IsNullOrWhiteSpace(request.SecondaryPhone) &&
                applicationUser.SecondaryPhoneNumber != request.SecondaryPhone)
                applicationUser.SecondaryPhoneNumber = request.SecondaryPhone;

            var result = await _userManager.UpdateAsync(applicationUser);

            if (result.Succeeded)
                return Result.Success(_mapper.Map<Result>(result));
            return Result.Error("Unable to update user details.");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to update user details.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    #endregion

    public async Task<Result<List<ApplicationUser>>> GetAllUsersAsync(QueryOptions options = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _userManager.Users.AsNoTracking().Include(u => u.Roles).ThenInclude(ur => ur.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(options?.SearchTerm))
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{options.SearchTerm}%"));

            if (!string.IsNullOrWhiteSpace(options?.OrderBy)) query = query.OrderBy(options.OrderBy);

            if (options?.Skip != null && options?.PageSize != null)
            {
                query = query.Skip(options.Skip);
                query = query.Take(options.PageSize);
            }

            return Result.Success(await query.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to create user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    private IQueryable<ApplicationUser> GetUsersWithRoles()
    {
        return _userManager.Users.AsNoTracking().Include(u => u.Roles).ThenInclude(ur => ur.Role);
    }

    private Result ValidateUserForActivation(ApplicationUser? applicationUser)
    {
        if (applicationUser == null)
            return Result.Error("User not found.");

        if (applicationUser.IsActive)
            return Result.Error("User is already active.");
        return Result.Success();
    }

    private async Task<Result> ValidateRolesForAdditionAsync(List<string> roles, ApplicationUser applicationUser)
    {
        var rolesAlreadyAssigned = await GetAlreadyAssignedRolesFromUserAsync(roles, applicationUser);
        var invalidRoles = GetInvalidRoles(roles);

        if (rolesAlreadyAssigned.Any())
            return Result.Error(rolesAlreadyAssigned.Select(r => $"Role {r} is already assigned.").ToArray());

        if (invalidRoles.Any())
            return Result.Error(invalidRoles.Select(r => $"Role {r} is invalid.").ToArray());

        return Result.Success();
    }

    private async Task<Result> ValidateRolesForRemovalAsync(List<string> roles, ApplicationUser applicationUser)
    {
        var invalidRoles = GetInvalidRoles(roles);
        var rolesNotAssigned = await GetUnassignedRolesFromUserAsync(roles, applicationUser);

        if (rolesNotAssigned.Any())
            return Result.Error(rolesNotAssigned.Select(r => $"Role {r} is not assigned.").ToArray());

        if (invalidRoles.Any())
            return Result.Error(invalidRoles.Select(r => $"Role {r} is invalid.").ToArray());

        return Result.Success();
    }

    private List<string> GetInvalidRoles(List<string> rolesToCheck)
    {
        var invalidRoles = new List<string>();
        foreach (var roleToCheck in rolesToCheck)
        {
            var isRoleInvalid = !Enum.IsDefined(typeof(RolesEnum), roleToCheck);
            if (isRoleInvalid)
                invalidRoles.Add(roleToCheck);
        }

        return invalidRoles;
    }

    private async Task<List<string>> GetUnassignedRolesFromUserAsync(List<string> rolesToCheck,
        ApplicationUser applicationUser)
    {
        var rolesNotAssigned = new List<string>();
        foreach (var roleToCheck in rolesToCheck)
        {
            var isRoleAssigned = await _userManager.IsInRoleAsync(applicationUser, roleToCheck.ToUpper());
            if (isRoleAssigned == false)
                rolesNotAssigned.Add(roleToCheck);
        }

        return rolesNotAssigned;
    }

    private async Task<List<string>> GetAlreadyAssignedRolesFromUserAsync(List<string> rolesToCheck,
        ApplicationUser applicationUser)
    {
        var rolesAssigned = new List<string>();
        foreach (var roleToCheck in rolesToCheck)
            if (await _userManager.IsInRoleAsync(applicationUser, roleToCheck.ToUpper()))
                rolesAssigned.Add(roleToCheck);
        return rolesAssigned;
    }
}