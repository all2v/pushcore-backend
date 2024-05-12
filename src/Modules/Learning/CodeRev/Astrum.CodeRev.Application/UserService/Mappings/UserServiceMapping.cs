using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Contest;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Interviews;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Tasks;
using Astrum.CodeRev.Domain.Aggregates;
using AutoMapper;

namespace Astrum.CodeRev.Application.UserService.Mappings;

public class UserServiceMapping : Profile
{
    public UserServiceMapping()
    {
        CreateMap<Interview, InterviewCreationDto>().ReverseMap();
        CreateMap<InterviewSolution, InterviewSolutionDto>()
            .ForMember(info => info.InterviewSolutionId, opt => opt.MapFrom(solution => solution.Id))
            .ForMember(info => info.InterviewId, opt => opt.MapFrom(solution => solution.Interview.Id))
            .ForMember(info => info.Vacancy, opt => opt.MapFrom(solution => solution.Interview.Vacancy))
            .ForMember(info => info.FullName,
                opt => opt.MapFrom(interviewSolution => $"{interviewSolution.Surname} {interviewSolution.FirstName}"))
            .ReverseMap();
        CreateMap<TaskSolution, TaskSolutionInfoContest>()
            .ForMember(contest => contest.TaskText,opt=>opt.MapFrom(solution =>solution.TestTask.TaskText ))
            .ForMember(contest => contest.TaskId,opt=>opt.MapFrom(solution =>solution.TestTask.Id ))
            .ForMember(contest => contest.StartCode,opt=>opt.MapFrom(solution =>solution.TestTask.StartCode ))
            .ForMember(contest => contest.ProgrammingLanguage,opt=>opt.MapFrom(solution =>solution.TestTask.ProgrammingLanguage ))
            .ReverseMap();
        CreateMap<TaskCreationDto, TestTask>()
            .ReverseMap();

        CreateMap<TestTask, TestTaskDto>().ReverseMap();
        CreateMap<TaskSolution,TaskSolutionDto>().ReverseMap();
        CreateMap<TaskSolutionDto,TaskSolutionInfoContest>()
            .ReverseMap();
    }
}