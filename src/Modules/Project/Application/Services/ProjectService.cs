using Ardalis.Specification;
using Astrum.Module.Project.Application.ViewModels;
using Astrum.Module.Project.Application.ViewModels.DTO;
using Astrum.Project.ViewModels.DTO;
using Astrum.Projects.Aggregates;
using Astrum.Projects.Repositories;
using Astrum.Projects.Specifications.Customer;
using Astrum.Projects.ViewModels;
using Astrum.Projects.ViewModels.DTO;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Projects.Services;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly IMemberRepository _memberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProductRepository _productRepository;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper,
        IMemberRepository memberRepository, IProductRepository productRepository)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _memberRepository = memberRepository;
        _productRepository = productRepository;
    }

    #region IProjectService Members

    public async Task<Result<ProjectView>> Create(ProjectRequest project)
    {
        var validationResult = Validate(project.StartDate.Date, project.EndDate?.Date, project.Name, project.Members);
        if (validationResult.Failed) 
            return Result<ProjectView>.Error(validationResult.MessageWithErrors);

        var newProject = _mapper.Map<Aggregates.Project>(project);
        await _projectRepository.AddAsync(newProject);
        try
        {
            await _projectRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании проекта");
        }
        var result = _mapper.Map<ProjectView>(newProject);
        return Result.Success(result);
    }

    public async Task<Result<List<ProjectView>>> GetAll()
    {
        var spec = new GetProjectsSpec();
        var projects = await _projectRepository.ListAsync(spec);
        var result = _mapper.Map<List<ProjectView>>(projects);
        return Result.Success(result);
    }

    public async Task<Result<ProjectView>> Delete(Guid id)
    {
        var spec = new GetProjectByIdSpec(id);

        var project = await _projectRepository.FirstOrDefaultAsync(spec);
        if (project == null)
            return Result.NotFound("Проект не найден.");
        try
        {
            await _projectRepository.DeleteAsync(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении проекта.");
        }
        return Result.Success(_mapper.Map<ProjectView>(project));
    }

    public async Task<Result<ProjectView>> Update(Guid id, ProjectUpdateDto projectUpdateDto)
    {
        var validationResult = Validate(projectUpdateDto.StartDate.Date, projectUpdateDto.EndDate?.Date,
            projectUpdateDto.Name, projectUpdateDto.Members);
        if (validationResult.Failed)
            return Result<ProjectView>.Error(validationResult.MessageWithErrors);

        var spec = new GetProjectByIdSpec(id);

        var project = await _projectRepository.FirstOrDefaultAsync(spec);
        var projectEdited = _mapper.Map<Aggregates.Project>(projectUpdateDto);
        if (project == null)
            return Result.NotFound("Проект не найден.");
        project.Name = projectEdited.Name;
        project.StartDate = projectEdited.StartDate;
        project.EndDate = projectEdited.EndDate;
        project.Description = projectEdited.Description;
        project.ProductId = projectEdited.ProductId;

        project.CustomFields = projectEdited.CustomFields;
        //project.Members = projectEdited.Members;
        if (projectUpdateDto != null)
            foreach (var member in projectUpdateDto.Members) 
                await UpdateMemberProject(member.UserId, member, id);
        try
        {
            await _projectRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении проекта.");
        }

        var updateResponse = _mapper.Map<ProjectView>(project);
        return Result.Success(updateResponse);
    }

    public async Task<Result<List<ProjectView>>> GetByMember(Guid memberId)
    {
        var spec = new GetProjectsByMemberIdSpec(memberId);
        var projects = await _projectRepository.ListAsync(spec);
        if (!projects.Any())
            return Result.NotFound("Проекты не найдены.");
        var result = _mapper.Map<List<ProjectView>>(projects);
        return Result.Success(result);
    }

    public async Task<Result<List<MemberShortView>>> GetMemberShortInfo(Guid memberId)
    {
        var spec = new GetProjectsByMemberIdSpec(memberId);
        var projects = await _projectRepository.ListAsync(spec);
        if (!projects.Any())
            return Result.NotFound("Проекты не найдены.");
        var result = _mapper.Map<List<MemberShortView>>(projects);
        foreach(var projectMember in result)
        {
            var project = projects.FirstOrDefault(project => project.Id == projectMember.ProjectId);
            projectMember.Role = project.Members.FirstOrDefault(member => member.UserId == memberId).Role;
        }
        return Result.Success(result);
    }

    public async Task<Result<ProjectView>> Get(Guid id)
    {
        var spec = new GetProjectByIdSpec(id);
        var result = await _projectRepository.FirstOrDefaultAsync(spec);
        if (result == null)
            return Result.NotFound("Проект не найден.");

        var projectView = _mapper.Map<ProjectView>(result);
        return Result.Success(projectView);
    } 

    public async Task<Result<ProjectView>> GetWithProductName(Guid id)
    {
        var spec = new GetProjectByIdSpec(id);
        var result = await _projectRepository.FirstOrDefaultAsync(spec);
        if (result == null)
            return Result.NotFound("Проект не найден.");

        var projectView = _mapper.Map<ProjectView>(result);
        var productSpec = new GetProductByIdSpec(result.ProductId);
        var product = await _productRepository.FirstOrDefaultAsync(productSpec);
        projectView.ProductName = product.Name;
        projectView.IsDeletable = product.Projects.Count > 1;
        return Result.Success(projectView);
    }

    #endregion
    #region ChangeMembers

    public async Task<Result<ProjectView>> AddMembers(AddMembersDto addMembersDto)
    {
        var spec = new GetProjectByIdSpec(addMembersDto.ProjectId);

        var project = await _projectRepository.FirstOrDefaultAsync(spec);
        if (project == null)
            return Result.NotFound("Проект не найден.");

        var addMembers = addMembersDto.Members.Select(_mapper.Map<Member>);
        var members = addMembers.Concat(project.Members);
        foreach (var member in members)
        {
            var sameMembers = members.Where(x => x.UserId == member.UserId);
            if (sameMembers.Count() > 1 && sameMembers.Count(x => member.Role == x.Role) > 1)
            {
                return Result.Error("Ошибка при создании одинаковых пользователей с одинаковыми ролями");
            }
        }
        project.Members.AddRange(addMembers);

        await _projectRepository.UpdateAsync(project);
        try
        {
            await _projectRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при добавлении членов проекта.");
        }

        var updateResponse = _mapper.Map<ProjectView>(project);
        return Result.Success(updateResponse);
    }

    public async Task<Result<MemberView>> UpdateMemberProject(Guid id, MemberRequest memberRequest, Guid projectId)
    {
        var spec = new GetMembersByIdSpec(id);

        var member = await _memberRepository.FirstOrDefaultAsync(spec);
        var memberEdited = _mapper.Map<Member>(memberRequest);
        if (member == null)
        {
            return Result.NotFound("Член проекта не найден.");
        }

        member.UserId = memberEdited.UserId;
        member.Role = memberEdited.Role;
        member.ProjectId = projectId;
        member.IsManager = memberEdited.IsManager;

        try
        {
            await _memberRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении члена проекта.");
        }

        var updateResponse = _mapper.Map<MemberView>(member);
        return Result.Success(updateResponse);
    }

    public async Task<Result<List<MemberView>>> RemoveMembers(RemoveMembersDto removeMembersDto)
    {
        var spec = new GetByProjectIdSpec(removeMembersDto.ProjectId);

        var members = await _memberRepository.ListAsync(spec);
        var membersToDelete = members.Where(e=>removeMembersDto.UsersId.Contains(e.UserId));
        try
        {
            await _memberRepository.DeleteRangeAsync(membersToDelete);
            await _memberRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении членов проекта.");
        }

        return Result.Success(_mapper.Map<List<MemberView>>(membersToDelete.ToList()));
    }

    #endregion

    private static Result Validate(DateTime startDate, DateTime? endDate, string name, List<MemberRequest> members)
    {
        if (startDate.Year < 2009)
        {
            return Result.Error("Некорректная дата начала");
        }

        if (endDate != null && startDate > endDate)
        {
            return Result.Error("Дата начала не может быть позже даты окончания");
        }

        if (name.Length > 30)
        {
            return Result.Error("Слишком длинное название проекта");
        }

        foreach (var member in members)
        {
            var sameMembers = members.Where(x => x.UserId == member.UserId);
            if (sameMembers.Count() > 1 && sameMembers.Count(x => member.Role == x.Role) > 1)
            {
                return Result.Error("Ошибка при создании одинаковых пользователей с одинаковыми ролями");
            }
        }

        return Result.Success();
    }
}