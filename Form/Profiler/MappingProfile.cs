using AutoMapper;
using Form.DTO;
using Form.Model;

namespace Form.Profiler
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<QuestionaireDTO, QuestionaireModel>().ForMember(dest => dest.questions, opt => opt.MapFrom(source => source.questionDTOs)).ReverseMap();
            CreateMap<QuestionDTO, QuestionModel>().ReverseMap();
            CreateMap<QuestionAnswerDTO,Response>().ReverseMap();
        }
    }
}
