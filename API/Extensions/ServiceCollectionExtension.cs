using Business;
using Business._Artifacts;
using Business.Burns;
using Business.Characters;
using Business.Equipments;
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
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IArtifactService, ArtifactService>();
        services.AddScoped<ICharacterArtifactService, CharacterArtifactService>();
        //services.AddScoped<INPCArtifactService, NPCArtifactService>();
        services.AddScoped<ICharacterService, CharacterService>();
        services.AddScoped<IAttributeService, AttributeService>();
        services.AddScoped<IBackgroundService, Business.Characters.BackgroundService>();
        services.AddScoped<ICharacterAttributeService, CharacterAttributeService>();
        services.AddScoped<ICharacterBackgroundService, CharacterBackgroundService>();
        services.AddScoped<ICharacterPotentialService, CharacterPotentialService>();
        services.AddScoped<ICharacterSkillService, CharacterSkillService>();
        services.AddScoped<IConceptService, ConceptService>();
        services.AddScoped<ICultService, CultService>();
        services.AddScoped<ICultureService, CultureService>();
        services.AddScoped<IPotentialService, PotentialService>();
        services.AddScoped<IRankService, RankService>();
        services.AddScoped<IRankPrerequisiteService, RankPrerequisiteService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IBurnService, BurnService>();
        //services.AddScoped<ICharacterBurnService, CharacterBurnService>();
        //services.AddScoped<INPCBurnService, NPCBurnService>();
        //services.AddScoped<ICharacterEquipmentService, CharacterEquipmentService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
        //services.AddScoped<INPCEquipmentService,NPCEquipmentService>();
        //services.AddScoped<ICharacterProtectionService, CharacterProtectionService>();
        //services.AddScoped<INPCProtectionService, NPCProtectionService>();
        services.AddScoped<IProtectionService, ProtectionService>();
        services.AddScoped<IProtectionQualityService, ProtectionQualityService>();
        //services.AddScoped<ICharacterVehicleService, CharacterVehicleService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IVehicleQualityService, VehicleQualityService>();
        services.AddScoped<IVehicleTypeService, VehicleTypeService>();
        //services.AddScoped<ICharacterWeaponService, CharacterWeaponService>();
        //services.AddScoped<INPCWeaponService, NPCWeaponService>();
        services.AddScoped<IWeaponService, WeaponService>();
        services.AddScoped<IWeaponQualityService, WeaponQualityService>();
        services.AddScoped<IWeaponTypeService, WeaponTypeService>();
        //services.AddScoped<INPCService, NPCService>();
        //services.AddScoped<INPCAttributeService, NPCAttributeService>();
        //services.AddScoped<INPCPotentialService, NPCPotentialService>();
        //services.AddScoped<INPCSkillService, NPCSkillService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IPotentialPrerequisiteService, PotentialPrerequisiteService>();
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