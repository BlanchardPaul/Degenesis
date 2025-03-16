using AutoMapper;
using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.Shared.DTOs.Burns;
using Degenesis.Shared.DTOs.Characters;
using Degenesis.Shared.DTOs.Equipments;
using Degenesis.Shared.DTOs.Protections;
using Degenesis.Shared.DTOs.Vehicles;
using Degenesis.Shared.DTOs.Weapons;
using Domain._Artifacts;
using Domain.Burns;
using Domain.Characters;
using Domain.Equipments;
using Domain.Protections;
using Domain.Vehicles;
using Domain.Weapons;

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

        CreateMap<CultureCreateDto, Culture>()
          .ForMember(dest => dest.AvailableCults, opt => opt.Ignore())
          .ForMember(dest => dest.BonusAttributes, opt => opt.Ignore())
          .ForMember(dest => dest.BonusSkills, opt => opt.Ignore());
        CreateMap<CultureDto, Culture>();
        CreateMap<Culture, CultureDto>()
            .ForMember(dest => dest.AvailableCults, opt => opt.MapFrom(src => src.AvailableCults))
            .ForMember(dest => dest.BonusAttributes, opt => opt.MapFrom(src => src.BonusAttributes))
            .ForMember(dest => dest.BonusSkills, opt => opt.MapFrom(src => src.BonusSkills));
        CreateMap<Cult, CultDto>();
        CreateMap<CultDto, Cult>();

        CreateMap<EquipmentCreateDto, Equipment>()
         .ForMember(dest => dest.EquipmentType, opt => opt.Ignore());
        CreateMap<EquipmentDto, Equipment>();
        CreateMap<Equipment, EquipmentDto>()
            .ForMember(dest => dest.EquipmentType, opt => opt.MapFrom(src => src.EquipmentType));

        CreateMap<EquipmentTypeCreateDto, EquipmentType>();
        CreateMap<EquipmentTypeDto, EquipmentType>();
        CreateMap<EquipmentType, EquipmentTypeDto>();

        CreateMap<PotentialCreateDto, Potential>()
            .ForMember(dest => dest.Cult, opt => opt.Ignore()); // Ignorer Cult car il sera chargé séparément
        CreateMap<PotentialDto, Potential>();
        CreateMap<Potential, PotentialDto>()
            .ForMember(dest => dest.Cult, opt => opt.MapFrom(src => src.Cult));

        CreateMap<ProtectionCreateDto, Protection>()
          .ForMember(dest => dest.Qualities, opt => opt.Ignore()); 
        CreateMap<ProtectionDto, Protection>();
        CreateMap<Protection, ProtectionDto>()
            .ForMember(dest => dest.Qualities, opt => opt.MapFrom(src => src.Qualities));


        CreateMap<ProtectionQualityCreateDto, ProtectionQuality>();
        CreateMap<ProtectionQualityDto, ProtectionQuality>();
        CreateMap<ProtectionQuality, ProtectionQualityDto>();

        CreateMap<RankCreateDto, Rank>()
           .ForMember(dest => dest.Prerequisites, opt => opt.Ignore())
           .ForMember(dest => dest.Cult, opt => opt.Ignore());
        CreateMap<RankDto, Rank>();
        CreateMap<Rank, RankDto>()
            .ForMember(dest => dest.Prerequisites, opt => opt.MapFrom(src => src.Prerequisites))
            .ForMember(dest => dest.Cult, opt => opt.MapFrom(src => src.Cult));

        CreateMap<RankPrerequisiteCreateDto, RankPrerequisite>()
            .ForMember(dest => dest.AttributeRequired, opt => opt.Ignore())
            .ForMember(dest => dest.SkillRequired, opt => opt.Ignore());
        CreateMap<RankPrerequisiteDto, RankPrerequisite>();
        CreateMap<RankPrerequisite, RankPrerequisiteDto>()
            .ForMember(dest => dest.AttributeRequired, opt => opt.MapFrom(src => src.AttributeRequired))
            .ForMember(dest => dest.SkillRequired, opt => opt.MapFrom(src => src.SkillRequired));

        CreateMap<SkillCreateDto, Skill>();
        CreateMap<Skill, Skill>();
        CreateMap<Skill, SkillDto>();
        CreateMap<SkillDto, Skill>();

        CreateMap<VehicleCreateDto, Vehicle>()
        .ForMember(dest => dest.VehicleType, opt => opt.Ignore());
        CreateMap<VehicleDto, Vehicle>();
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType));

        CreateMap<VehicleTypeCreateDto, VehicleType>();
        CreateMap<VehicleTypeDto, VehicleType>();
        CreateMap<VehicleType, VehicleTypeDto>();

        CreateMap<WeaponCreateDto, Weapon>();
        CreateMap<WeaponDto, Weapon>();
        CreateMap<Weapon, WeaponDto>()
            .ForMember(dest => dest.WeaponType, opt => opt.MapFrom(src => src.WeaponType))
            .ForMember(dest => dest.Attribute, opt => opt.MapFrom(src => src.Attribute))
            .ForMember(dest => dest.Qualities, opt => opt.MapFrom(src => src.Qualities));

        CreateMap<WeaponQualityCreateDto, WeaponQuality>();
        CreateMap<WeaponQualityDto, WeaponQuality>();
        CreateMap<WeaponQuality, WeaponQualityDto>();

        CreateMap<WeaponTypeCreateDto, WeaponType>();
        CreateMap<WeaponTypeDto, WeaponType>();
        CreateMap<WeaponType, WeaponTypeDto>();
    }
}