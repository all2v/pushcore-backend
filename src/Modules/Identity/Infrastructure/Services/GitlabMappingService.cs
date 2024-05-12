using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Astrum.Identity.Application.Contracts;
using Astrum.Identity.Application.Repositories;
using Astrum.Identity.Domain.Entities;
using Astrum.Identity.Features.Commands.Register;
using Astrum.Identity.Models;
using Astrum.Identity.Repositories;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Identity.Infrastructure.Services
{
    public class GitlabMappingService : IGitlabMappingService
    {
        private readonly IGitLabMappingsRepository _gitLabMappingsRepository;
        private readonly IGitLabUsersRepository _gitLabUsersRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ISender _mediator;
        private readonly IPasswordGeneratorService _passwordGenerator;

        public GitlabMappingService(IGitLabMappingsRepository gitLabMappingsRepository,
            IGitLabUsersRepository gitLabUsersRepository,
            IApplicationUserRepository applicationUserRepository,
            ISender mediator, IPasswordGeneratorService passwordGenerator)
        {
            _gitLabMappingsRepository = gitLabMappingsRepository;
            _gitLabUsersRepository = gitLabUsersRepository;
            _applicationUserRepository = applicationUserRepository;
            _mediator = mediator;
            _passwordGenerator = passwordGenerator;
        }

        public async Task<ApplicationUser> GetApplicationUserByGitlabId(long gitlabId) 
        {
            var mapping = await _gitLabMappingsRepository.Items
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.GitlabUserId == gitlabId);
            if (mapping == null || mapping.User == null)
                return null;
            return mapping.User;
        }

        //TODO: return result with notification wheather username and email was already taken
        public async Task<Guid> AddUserFromGitLab(GitlabUser gitlabUser)
        {
            var usernameExists = await _applicationUserRepository.AnyAsync(u => u.UserName.ToLower() == gitlabUser.Username.ToLower());
            var emailExists = await _applicationUserRepository.AnyAsync(u => u.Email.ToLower() == gitlabUser.Email.ToLower());

            //TODO: add exist notification to result

            var command = new RegisterCommand
            {
                Username = usernameExists ? gitlabUser.Username + "_GitLab_" + gitlabUser.Id.ToString() : gitlabUser.Username,
                Name = gitlabUser.Name,
                Email = emailExists ? "" : gitlabUser.Email,
                //TODO: discuss behaviour
                Password = _passwordGenerator.GenerateRandomPassword()
            };
            var response = await _mediator.Send(command);
            var newMapping = new GitLabUsersMappings { UserID = response.Data.Id, GitlabUser = gitlabUser };
            await _gitLabMappingsRepository.AddAsync(newMapping);
            await _gitLabMappingsRepository.UnitOfWork.SaveChangesAsync();
            return response.Data.Id;
        }
    }
}
