using Business;
using Business._Artifacts;
using Business.Burns;
using Business.Characters;
using Business.Characters.Inventory;
using Business.Equipments;
using Business.Hubs;
using Business.NPCs;
using Business.Protections;
using Business.Rooms;
using Business.Users;
using Business.Vehicles;
using Business.Weapons;
using DataAccessLayer;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        services.AddScoped<IArtifactService, ArtifactService>();
        services.AddScoped<IAttributeService, AttributeService>();
        services.AddScoped<IBackgroundService, Business.Characters.BackgroundService>();
        services.AddScoped<IBaseNPCService, BaseNPCService>();
        services.AddScoped<IBurnService, BurnService>();
        services.AddScoped<ICharacterArtifactService, CharacterArtifactService>();
        services.AddScoped<ICharacterService, CharacterService>();
        services.AddScoped<ICharacterAttributeService, CharacterAttributeService>();
        services.AddScoped<ICharacterBackgroundService, CharacterBackgroundService>();
        services.AddScoped<ICharacterBurnService, CharacterBurnService>();
        services.AddScoped<ICharacterEquipmentService, CharacterEquipmentService>();
        services.AddScoped<ICharacterPotentialService, CharacterPotentialService>();
        services.AddScoped<ICharacterProtectionService, CharacterProtectionService>();
        services.AddScoped<ICharacterSavedDiceRollService, CharacterSavedDiceRollService>();
        services.AddScoped<ICharacterSkillService, CharacterSkillService>();
        services.AddScoped<ICharacterVehicleService, CharacterVehicleService>();
        services.AddScoped<ICharacterWeaponService, CharacterWeaponService>();
        services.AddScoped<IConceptService, ConceptService>();
        services.AddScoped<ICultService, CultService>();
        services.AddScoped<ICultureService, CultureService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
        services.AddScoped<INPCService, NPCService>();
        services.AddScoped<IPotentialService, PotentialService>();
        services.AddScoped<IPotentialPrerequisiteService, PotentialPrerequisiteService>();
        services.AddScoped<IProtectionService, ProtectionService>();
        services.AddScoped<IRankService, RankService>();
        services.AddScoped<IRankPrerequisiteService, RankPrerequisiteService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRoomBroadcastService, RoomBroadcastService>();
        services.AddScoped<IRoomCultRelationshipService, RoomCultRelationshipService>();
        services.AddScoped<IRoomChatMessageService, RoomChatMessageService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IVehicleTypeService, VehicleTypeService>();
        services.AddScoped<IWeaponService, WeaponService>();
        services.AddScoped<IWeaponTypeService, WeaponTypeService>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddIdentity<ApplicationUser, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
            };
        });

        builder.Services.AddAuthorization();
        builder.Services.AddSignalR();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFront", policy =>
            {
                policy.WithOrigins("https://localhost:7186")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });


        return services;
    }
}