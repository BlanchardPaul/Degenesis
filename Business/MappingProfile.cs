using AutoMapper;
using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.Shared.DTOs.Burns;
using Degenesis.Shared.DTOs.Characters;
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
        CreateMap<Artifact, Artifact>();

        CreateMap<AttributeCreateDto, CAttribute>();
        CreateMap<CAttribute, AttributeDto>();
        CreateMap<AttributeDto, CAttribute>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore()); ;

        CreateMap<BackgroundCreateDto, Background>();
        CreateMap<Background, BackgroundDto>();

        CreateMap<BurnCreateDto, Burn>();
        CreateMap<Burn, Burn>();

        CreateMap<ConceptCreateDto, Concept>();
        CreateMap<Concept, Concept>();
        CreateMap<Concept, ConceptDto>()
                   .ForMember(dest => dest.BonusAttribute, opt => opt.MapFrom(src => src.BonusAttribute)) // BonusAttribute → AttributeDto
                   .ForMember(dest => dest.BonusSkills, opt => opt.MapFrom(src => src.BonusSkills)); // List<Skill> → List<SkillDto>

        CreateMap<CharacterCreateDto, Character>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Generated in DB
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


        CreateMap<CharacterAttributeDto, CharacterAttribute>();
        CreateMap<CharacterSkillDto, CharacterSkill>();
        CreateMap<CharacterBackgroundDto, CharacterBackground>();
        CreateMap<CharacterPotentialDto, CharacterPotential>();
        CreateMap<CharacterPotential, CharacterPotentialDto>();

        CreateMap<Character, CharacterDto>()
            .ForMember(dest => dest.Cult, opt => opt.MapFrom(src => src.Cult))
            .ForMember(dest => dest.Culture, opt => opt.MapFrom(src => src.Culture))
            .ForMember(dest => dest.Concept, opt => opt.MapFrom(src => src.Concept))
            .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Rank))
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src =>
                src.CharacterAttributes.Select(a => new CharacterAttributeDto
                {
                    CharacterId = a.CharacterId,
                    AttributeId = a.AttributeId,
                    Level = a.Level
                })))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
                src.CharacterSkills.Select(s => new CharacterSkillDto
                {
                    CharacterId = s.CharacterId,
                    SkillId = s.SkillId,
                    Level = s.Level
                })))
            .ForMember(dest => dest.Backgrounds, opt => opt.MapFrom(src =>
                src.CharacterBackgrounds.Select(b => new CharacterBackgroundDto
                {
                    CharacterId = b.CharacterId,
                    BackgroundId = b.BackgroundId,
                    Level = b.Level
                })))
            .ForMember(dest => dest.Potentials, opt => opt.MapFrom(src =>
                src.CharacterPontentials.Select(cp => new CharacterPotentialDto
                {
                    CharacterId = cp.CharacterId,
                    PotentialId = cp.PotentialId,
                    Level = cp.Level
                })));

        CreateMap<CultCreateDto, Cult>()
            .ForMember(dest => dest.BonusSkills, opt => opt.Ignore()); // Ignore to avoid creating Skills doubles
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
            .ForMember(dest => dest.Prerequisites, opt => opt.Ignore())
            .ForMember(dest => dest.Cult, opt => opt.Ignore());

        CreateMap<PotentialDto, Potential>()
            .ForMember(dest => dest.Cult, opt => opt.Ignore())
            .ForMember(dest => dest.Prerequisites, opt => opt.Ignore());

        CreateMap<Potential, PotentialDto>()
            .ForMember(dest => dest.Prerequisites, opt => opt.MapFrom(src => src.Prerequisites))
            .ForMember(dest => dest.Cult, opt => opt.MapFrom(src => src.Cult))
            .MaxDepth(2);

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
        CreateMap<ProtectionDto, Protection>();
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

        CreateMap<SkillCreateDto, Skill>();
        CreateMap<Skill, Skill>();
        CreateMap<Skill, SkillDto>();
        CreateMap<SkillDto, Skill>();

        CreateMap<RoomCreateDto, Room>()
            .ForMember(dest => dest.UserRooms, opt => opt.Ignore());
        CreateMap<Room, Room>();
        CreateMap<Room, RoomDto>();
        CreateMap<RoomDto, Room>()
            .ForMember(dest => dest.UserRooms, opt => opt.Ignore());

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

        CreateMap<UserCreateDto, ApplicationUser>();
    }
}