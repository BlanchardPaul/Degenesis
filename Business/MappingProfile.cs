using AutoMapper;
using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.Shared.DTOs.Burns;
using Domain._Artifacts;
using Domain.Burns;

namespace Business;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArtifactCreateDto, Artifact>();
        CreateMap<Artifact, Artifact>()
           .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<BurnCreateDto, Burn>();
        CreateMap<Burn, Burn>()
           .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}