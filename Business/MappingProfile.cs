using AutoMapper;
using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.Shared.DTOs.Burns;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Equipments;
using Degenesis.Shared.DTOs.Protections;
using Degenesis.Shared.DTOs.Rooms;
using Degenesis.Shared.DTOs.Users;
using Degenesis.Shared.DTOs.Vehicles;
using Degenesis.Shared.DTOs.Weapons;
using Domain._Artifacts;
using Domain.Burns;
using Domain.Characters;
using Domain.Equipments;
using Domain.Protections;
using Domain.Rooms;
using Domain.Users;
using Domain.Vehicles;
using Domain.Weapons;

namespace Business;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArtifactCreateDto, Artifact>();
        CreateMap<Artifact, ArtifactDto>();
        CreateMap<ArtifactDto, Artifact>();

        CreateMap<AttributeCreateDto, CAttribute>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore());
        CreateMap<AttributeDto, CAttribute>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore());
        CreateMap<CAttribute, AttributeDto>();


        CreateMap<BackgroundCreateDto, Background>();
        CreateMap<BackgroundDto, Background>();
        CreateMap<Background, BackgroundDto>();

        CreateMap<BurnCreateDto, Burn>();
        CreateMap<Burn, BurnDto>();
        CreateMap<BurnDto, Burn>();

        CreateMap<CharacterCreateDto, Character>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())// Generated in DB
            .ForMember(dest => dest.Cult, opt => opt.Ignore())
            .ForMember(dest => dest.Culture, opt => opt.Ignore())
            .ForMember(dest => dest.Concept, opt => opt.Ignore())
            .ForMember(dest => dest.Room, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore())
            .ForMember(dest => dest.IdApplicationUser, opt => opt.Ignore()) // Handled in the service where it can be retrieved in the context
            .ForMember(dest => dest.CharacterAttributes, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterSkills, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterBackgrounds, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterBurns, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterPontentials, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterProtections, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterEquipments, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterArtifacts, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterVehicles, opt => opt.Ignore())
            .ForMember(dest => dest.CharacterWeapons, opt => opt.Ignore())
            .ForMember(dest => dest.Rank, opt => opt.Ignore())
            .ForMember(dest => dest.Cult, opt => opt.Ignore());

        CreateMap<Character, CharacterDto>()
            .ForMember(dest => dest.Cult, opt => opt.MapFrom(src => src.Cult))
            .ForMember(dest => dest.Culture, opt => opt.MapFrom(src => src.Culture))
            .ForMember(dest => dest.Concept, opt => opt.MapFrom(src => src.Concept))
            .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Rank))
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.CharacterAttributes))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.CharacterSkills))
            .ForMember(dest => dest.Backgrounds, opt => opt.MapFrom(src => src.CharacterBackgrounds))
            .ForMember(dest => dest.Potentials, opt => opt.MapFrom(src => src.CharacterPontentials));

        CreateMap<Character, CharacterDisplayDto>()
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.CharacterAttributes))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.CharacterSkills))
            .ForMember(dest => dest.Backgrounds, opt => opt.MapFrom(src => src.CharacterBackgrounds))
            .ForMember(dest => dest.Potentials, opt => opt.MapFrom(src => src.CharacterPontentials));

        CreateMap<CharacterBasicInfosEditDto, Character>();

        CreateMap<CharacterArtifactCreateDto, CharacterArtifact>()
             .ForMember(dest => dest.Character, opt => opt.Ignore())
             .ForMember(dest => dest.Artifact, opt => opt.Ignore());
        CreateMap<CharacterArtifactDto, CharacterArtifact>()
             .ForMember(dest => dest.Character, opt => opt.Ignore())
             .ForMember(dest => dest.Artifact, opt => opt.Ignore());
        CreateMap<CharacterArtifact, CharacterArtifactDto>()
            .ForMember(dest => dest.Artifact, opt => opt.MapFrom(ca => ca.Artifact));

        CreateMap<CharacterAttributeDto, CharacterAttribute>()
            .ForMember(dest => dest.Character, opt => opt.Ignore())
            .ForMember(dest => dest.Attribute, opt => opt.Ignore());
        CreateMap<CharacterAttribute, CharacterAttributeDto>();
        CreateMap<CharacterAttribute, CharacterAttributeDisplayDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Attribute.Name));

        CreateMap<CharacterSkillDto, CharacterSkill>()
            .ForMember(dest => dest.Character, opt => opt.Ignore())
            .ForMember(dest => dest.Skill, opt => opt.Ignore());
        CreateMap<CharacterSkill, CharacterSkillDto>();
        CreateMap<CharacterSkill, CharacterSkillDisplayDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill.Name))
            .ForMember(dest => dest.AttributeId, opt => opt.MapFrom(src => src.Skill.CAttributeId));

        CreateMap<CharacterBackgroundDto, CharacterBackground>()
            .ForMember(dest => dest.Character, opt => opt.Ignore())
            .ForMember(dest => dest.Background, opt => opt.Ignore());
        CreateMap<CharacterBackground, CharacterBackgroundDto>();
        CreateMap<CharacterBackground, CharacterBackgroundDisplayDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Background.Name));

        CreateMap<CharacterPotentialDto, CharacterPotential>()
            .ForMember(dest => dest.Character, opt => opt.Ignore())
            .ForMember(dest => dest.Potential, opt => opt.Ignore());
        CreateMap<CharacterPotential, CharacterPotentialDto>();
        CreateMap<CharacterPotential, CharacterPotentialDisplayDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Potential.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Potential.Description));

        CreateMap<ConceptCreateDto, Concept>()
            .ForMember(dest => dest.BonusAttribute, opt => opt.Ignore())
            .ForMember(dest => dest.BonusSkills, opt => opt.Ignore());
        CreateMap<ConceptDto, Concept>()
            .ForMember(dest => dest.BonusAttribute, opt => opt.Ignore())
            .ForMember(dest => dest.BonusSkills, opt => opt.Ignore());
        CreateMap<Concept, ConceptDto>()
                   .ForMember(dest => dest.BonusAttribute, opt => opt.MapFrom(src => src.BonusAttribute))
                   .ForMember(dest => dest.BonusSkills, opt => opt.MapFrom(src => src.BonusSkills));

        CreateMap<CultCreateDto, Cult>()
            .ForMember(dest => dest.BonusSkills, opt => opt.Ignore());
        CreateMap<CultDto, Cult>()
            .ForMember(dest => dest.BonusSkills, opt => opt.Ignore());
        CreateMap<Cult, CultDto>()
            .ForMember(dest => dest.BonusSkills, opt => opt.MapFrom(src => src.BonusSkills));

        CreateMap<CultureCreateDto, Culture>()
            .ForMember(dest => dest.AvailableCults, opt => opt.Ignore())
            .ForMember(dest => dest.BonusAttributes, opt => opt.Ignore())
            .ForMember(dest => dest.BonusSkills, opt => opt.Ignore());
        CreateMap<CultureDto, Culture>()
            .ForMember(dest => dest.AvailableCults, opt => opt.Ignore())
            .ForMember(dest => dest.BonusAttributes, opt => opt.Ignore())
            .ForMember(dest => dest.BonusSkills, opt => opt.Ignore());
        CreateMap<Culture, CultureDto>()
            .ForMember(dest => dest.AvailableCults, opt => opt.MapFrom(src => src.AvailableCults))
            .ForMember(dest => dest.BonusAttributes, opt => opt.MapFrom(src => src.BonusAttributes))
            .ForMember(dest => dest.BonusSkills, opt => opt.MapFrom(src => src.BonusSkills));

        CreateMap<EquipmentCreateDto, Equipment>()
            .ForMember(dest => dest.EquipmentType, opt => opt.Ignore())
            .ForMember(dest => dest.Cults, opt => opt.Ignore());
        CreateMap<EquipmentDto, Equipment>()
            .ForMember(dest => dest.EquipmentType, opt => opt.Ignore())
            .ForMember(dest => dest.Cults, opt => opt.Ignore());
        CreateMap<Equipment, EquipmentDto>()
            .ForMember(dest => dest.EquipmentType, opt => opt.MapFrom(src => src.EquipmentType))
            .ForMember(dest => dest.Cults, opt => opt.MapFrom(src => src.Cults));

        CreateMap<EquipmentTypeCreateDto, EquipmentType>();
        CreateMap<EquipmentTypeDto, EquipmentType>();
        CreateMap<EquipmentType, EquipmentTypeDto>();

        CreateMap<PotentialCreateDto, Potential>()
            .ForMember(dest => dest.Prerequisites, opt => opt.Ignore())
            .ForMember(dest => dest.Cult, opt => opt.Ignore());

        CreateMap<PotentialDto, Potential>()
            .ForMember(dest => dest.Cult, opt => opt.Ignore())
            .ForMember(dest => dest.Prerequisites, opt => opt.Ignore());

        CreateMap<Potential, PotentialDto>()
            .ForMember(dest => dest.Prerequisites, opt => opt.MapFrom(src => src.Prerequisites))
            .ForMember(dest => dest.Cult, opt => opt.MapFrom(src => src.Cult));

        CreateMap<PotentialPrerequisiteCreateDto, PotentialPrerequisite>()
            .ForMember(dest => dest.AttributeRequired, opt => opt.Ignore())
            .ForMember(dest => dest.SkillRequired, opt => opt.Ignore())
            .ForMember(dest => dest.BackgroundRequired, opt => opt.Ignore())
            .ForMember(dest => dest.RankRequired, opt => opt.Ignore());

        CreateMap<PotentialPrerequisiteDto, PotentialPrerequisite>()
            .ForMember(dest => dest.AttributeRequired, opt => opt.Ignore())
            .ForMember(dest => dest.SkillRequired, opt => opt.Ignore())
            .ForMember(dest => dest.BackgroundRequired, opt => opt.Ignore())
            .ForMember(dest => dest.RankRequired, opt => opt.Ignore());

        CreateMap<PotentialPrerequisite, PotentialPrerequisiteDto>()
            .ForMember(dest => dest.AttributeRequired, opt => opt.MapFrom(src => src.AttributeRequired))
            .ForMember(dest => dest.SkillRequired, opt => opt.MapFrom(src => src.SkillRequired))
            .ForMember(dest => dest.BackgroundRequired, opt => opt.MapFrom(src => src.BackgroundRequired))
            .ForMember(dest => dest.RankRequired, opt => opt.MapFrom(src => src.RankRequired));

        CreateMap<ProtectionCreateDto, Protection>()
            .ForMember(dest => dest.Qualities, opt => opt.Ignore()); 
        CreateMap<ProtectionDto, Protection>()
            .ForMember(dest => dest.Qualities, opt => opt.Ignore());
        CreateMap<Protection, ProtectionDto>()
            .ForMember(dest => dest.Qualities, opt => opt.MapFrom(src => src.Qualities));


        CreateMap<ProtectionQualityCreateDto, ProtectionQuality>();
        CreateMap<ProtectionQualityDto, ProtectionQuality>();
        CreateMap<ProtectionQuality, ProtectionQualityDto>();

        CreateMap<RankCreateDto, Rank>()
            .ForMember(dest => dest.Prerequisites, opt => opt.Ignore())
            .ForMember(dest => dest.Cult, opt => opt.Ignore())
            .ForMember(dest => dest.ParentRank, opt => opt.Ignore());

        CreateMap<RankDto, Rank>()
            .ForMember(dest => dest.Cult, opt => opt.Ignore())
            .ForMember(dest => dest.Prerequisites, opt => opt.Ignore())
            .ForMember(dest => dest.ParentRank, opt => opt.Ignore());

        CreateMap<Rank, RankDto>()
            .ForMember(dest => dest.Prerequisites, opt => opt.MapFrom(src => src.Prerequisites))
            .ForMember(dest => dest.Cult, opt => opt.MapFrom(src => src.Cult))
            .ForMember(dest => dest.ParentRank, opt => opt.MapFrom(src => src.ParentRank))
            .MaxDepth(2);

        CreateMap<RankPrerequisiteCreateDto, RankPrerequisite>()
            .ForMember(dest => dest.AttributeRequired, opt => opt.Ignore())
            .ForMember(dest => dest.SkillRequired, opt => opt.Ignore())
            .ForMember(dest => dest.BackgroundRequired, opt => opt.Ignore());

        CreateMap<RankPrerequisiteDto, RankPrerequisite>()
            .ForMember(dest => dest.AttributeRequired, opt => opt.Ignore())
            .ForMember(dest => dest.SkillRequired, opt => opt.Ignore())
            .ForMember(dest => dest.BackgroundRequired, opt => opt.Ignore());

        CreateMap<RankPrerequisite, RankPrerequisiteDto>()
            .ForMember(dest => dest.AttributeRequired, opt => opt.MapFrom(src => src.AttributeRequired))
            .ForMember(dest => dest.SkillRequired, opt => opt.MapFrom(src => src.SkillRequired))
            .ForMember(dest => dest.BackgroundRequired, opt => opt.MapFrom(src => src.BackgroundRequired));

        CreateMap<SkillCreateDto, Skill>()
            .ForMember(dest => dest.CAttribute, opt => opt.Ignore());
        CreateMap<SkillDto, Skill>()
            .ForMember(dest => dest.CAttribute, opt => opt.Ignore());
        CreateMap<Skill, SkillDto>()
            .ForMember(dest => dest.CAttribute, opt => opt.MapFrom(src => src.CAttribute))
            .MaxDepth(2);

        CreateMap<RoomCreateDto, Room>()
            .ForMember(dest => dest.UserRooms, opt => opt.Ignore());
        CreateMap<Room, RoomDto>();
        CreateMap<RoomDto, Room>()
            .ForMember(dest => dest.UserRooms, opt => opt.Ignore());

        CreateMap<VehicleCreateDto, Vehicle>()
            .ForMember(dest => dest.VehicleQualities, opt => opt.Ignore())
            .ForMember(dest => dest.VehicleType, opt => opt.Ignore())
            .ForMember(dest => dest.Cult, opt => opt.Ignore());
        CreateMap<VehicleDto, Vehicle>()
            .ForMember(dest => dest.VehicleQualities, opt => opt.Ignore())
            .ForMember(dest => dest.VehicleType, opt => opt.Ignore())
            .ForMember(dest => dest.Cult, opt => opt.Ignore());
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.VehicleQualities, opt => opt.MapFrom(src => src.VehicleQualities))
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType))
            .ForMember(dest => dest.Cult, opt => opt.MapFrom(src => src.Cult));

        CreateMap<VehicleTypeCreateDto, VehicleType>();
        CreateMap<VehicleTypeDto, VehicleType>();
        CreateMap<VehicleType, VehicleTypeDto>();

        CreateMap<VehicleQualityCreateDto, VehicleQuality>();
        CreateMap<VehicleQualityDto, VehicleQuality>();
        CreateMap<VehicleQuality, VehicleQualityDto>();

        CreateMap<WeaponCreateDto, Weapon>()
            .ForMember(dest => dest.WeaponType, opt => opt.Ignore())
            .ForMember(dest => dest.Attribute, opt => opt.Ignore())
            .ForMember(dest => dest.Skill, opt => opt.Ignore())
            .ForMember(dest => dest.Qualities, opt => opt.Ignore())
            .ForMember(dest => dest.Cults, opt => opt.Ignore());
        CreateMap<WeaponDto, Weapon>()
            .ForMember(dest => dest.WeaponType, opt => opt.Ignore())
            .ForMember(dest => dest.Attribute, opt => opt.Ignore())
            .ForMember(dest => dest.Skill, opt => opt.Ignore())
            .ForMember(dest => dest.Qualities, opt => opt.Ignore())
            .ForMember(dest => dest.Cults, opt => opt.Ignore());
        CreateMap<Weapon, WeaponDto>()
            .ForMember(dest => dest.WeaponType, opt => opt.MapFrom(src => src.WeaponType))
            .ForMember(dest => dest.Attribute, opt => opt.MapFrom(src => src.Attribute))
            .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
            .ForMember(dest => dest.Qualities, opt => opt.MapFrom(src => src.Qualities))
            .ForMember(dest => dest.Cults, opt => opt.MapFrom(src => src.Cults));

        CreateMap<WeaponQualityCreateDto, WeaponQuality>();
        CreateMap<WeaponQualityDto, WeaponQuality>();
        CreateMap<WeaponQuality, WeaponQualityDto>();

        CreateMap<WeaponTypeCreateDto, WeaponType>();
        CreateMap<WeaponTypeDto, WeaponType>();
        CreateMap<WeaponType, WeaponTypeDto>();

        CreateMap<UserCreateDto, ApplicationUser>();
    }
}