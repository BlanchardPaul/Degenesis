using AutoMapper;
using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.Shared.DTOs.Burns;
using Degenesis.Shared.DTOs.Characters;
using Domain._Artifacts;
using Domain.Burns;
using Domain.Characters;

namespace Business;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArtifactCreateDto, Artifact>();
        CreateMap<Artifact, Artifact>();

        CreateMap<AttributeCreateDto, CAttribute>();
        CreateMap<CAttribute, CAttribute>();
        CreateMap<CAttribute, AttributeDto>();
        CreateMap<AttributeDto, CAttribute>();

        CreateMap<BackgroundCreateDto, Background>();
        CreateMap<Background, Background>();

        CreateMap<BurnCreateDto, Burn>();
        CreateMap<Burn, Burn>();

        CreateMap<ConceptCreateDto, Concept>();
        CreateMap<Concept, Concept>();
        CreateMap<Concept, ConceptDto>()
                   .ForMember(dest => dest.BonusAttribute, opt => opt.MapFrom(src => src.BonusAttribute)) // BonusAttribute → AttributeDto
                   .ForMember(dest => dest.BonusSkills, opt => opt.MapFrom(src => src.BonusSkills)); // List<Skill> → List<SkillDto>

        CreateMap<CultCreateDto, Cult>()
            .ForMember(dest => dest.BonusSkills, opt => opt.Ignore()); // Ignore pour éviter la création de doublons de skills
        CreateMap<Cult, Cult>();
        CreateMap<Cult, CultDto>();
        CreateMap<CultDto, Cult>();

        CreateMap<SkillCreateDto, Skill>();
        CreateMap<Skill, Skill>();
        CreateMap<Skill, SkillDto>();
        CreateMap<SkillDto, Skill>();

    }
}