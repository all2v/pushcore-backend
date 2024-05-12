using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using AutoMapper;

namespace Astrum.ITDictionary.Mappings;

public class ITDictionaryProfile: Profile
{
    public ITDictionaryProfile()
    {
        CreateMap<Term, TermView>();
        CreateMap<TermRequest, Term>();
        CreateMap<Category, TermsCategoryView>();
        CreateMap<CategoryRequest, Category>();
        CreateMap<Guid, Category>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s));

        CreateMap<CreatePracticeRequest, Practice>();

        CreateMap<TestQuestion, TestQuestionView>();
            // .ForMember(d => d.TermId);
        CreateMap<QuestionAnswerOption, AnswerOptionView>();
    }
}