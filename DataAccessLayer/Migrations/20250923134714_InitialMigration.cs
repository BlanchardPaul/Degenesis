using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artifacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EnergyStorage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Magazine = table.Column<int>(type: "int", nullable: false),
                    Encumbrance = table.Column<int>(type: "int", nullable: false),
                    Activation = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Backgrounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backgrounds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Burns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Chakra = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EarthChakra = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Effect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeakCost = table.Column<int>(type: "int", nullable: false),
                    PotentCost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Burns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NPCs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Ego = table.Column<int>(type: "int", nullable: false),
                    FleshWounds = table.Column<int>(type: "int", nullable: false),
                    Trauma = table.Column<int>(type: "int", nullable: false),
                    PassiveDefense = table.Column<int>(type: "int", nullable: false),
                    EnemySpec = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionQualities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionQualities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Protections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: true),
                    Stockage = table.Column<int>(type: "int", nullable: false),
                    Slots = table.Column<int>(type: "int", nullable: false),
                    Connectors = table.Column<int>(type: "int", nullable: false),
                    Consuption = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Defense = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Attack = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Encumbrance = table.Column<int>(type: "int", nullable: false),
                    TechLevel = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Resources = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponQualities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponQualities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Concepts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    BonusAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concepts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concepts_Attributes_BonusAttributeId",
                        column: x => x.BonusAttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Attributes_CAttributeId",
                        column: x => x.CAttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Potentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Prerequisite = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Potentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Potentials_Cults_CultId",
                        column: x => x.CultId,
                        principalTable: "Cults",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Equipment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ranks_Cults_CultId",
                        column: x => x.CultId,
                        principalTable: "Cults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CAttributeCulture",
                columns: table => new
                {
                    BonusAttributesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CultureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAttributeCulture", x => new { x.BonusAttributesId, x.CultureId });
                    table.ForeignKey(
                        name: "FK_CAttributeCulture_Attributes_BonusAttributesId",
                        column: x => x.BonusAttributesId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CAttributeCulture_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CultCulture",
                columns: table => new
                {
                    AvailableCultsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CultureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultCulture", x => new { x.AvailableCultsId, x.CultureId });
                    table.ForeignKey(
                        name: "FK_CultCulture_Cults_AvailableCultsId",
                        column: x => x.AvailableCultsId,
                        principalTable: "Cults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultCulture_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Capacity = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Effect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Encumbrance = table.Column<int>(type: "int", nullable: false),
                    TechLevel = table.Column<int>(type: "int", nullable: false),
                    Slots = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Resources = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnergyStorage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipmentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCArtifacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    NPCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtifactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChargeInMagazine = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCArtifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NPCArtifacts_Artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCArtifacts_NPCs_NPCId",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCAttributes",
                columns: table => new
                {
                    NPCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCAttributes", x => new { x.NPCId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_NPCAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCAttributes_NPCs_NPCId",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCBurns",
                columns: table => new
                {
                    NPCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BurnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCBurns", x => new { x.NPCId, x.BurnId });
                    table.ForeignKey(
                        name: "FK_NPCBurns_Burns_BurnId",
                        column: x => x.BurnId,
                        principalTable: "Burns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCBurns_NPCs_NPCId",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCProtections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NPCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProtectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsedConnectors = table.Column<int>(type: "int", nullable: false),
                    UsedSlots = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCProtections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NPCProtections_NPCs_NPCId",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCProtections_Protections_ProtectionId",
                        column: x => x.ProtectionId,
                        principalTable: "Protections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionProtectionQuality",
                columns: table => new
                {
                    ProtectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QualitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionProtectionQuality", x => new { x.ProtectionId, x.QualitiesId });
                    table.ForeignKey(
                        name: "FK_ProtectionProtectionQuality_ProtectionQualities_QualitiesId",
                        column: x => x.QualitiesId,
                        principalTable: "ProtectionQualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProtectionProtectionQuality_Protections_ProtectionId",
                        column: x => x.ProtectionId,
                        principalTable: "Protections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdApplicationUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdRoom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsGM = table.Column<bool>(type: "bit", nullable: false),
                    InvitationAccepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRooms_AspNetUsers_IdApplicationUser",
                        column: x => x.IdApplicationUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRooms_Rooms_IdRoom",
                        column: x => x.IdRoom,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false),
                    Acceleration = table.Column<int>(type: "int", nullable: false),
                    Brake = table.Column<int>(type: "int", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false),
                    BodyFlesh = table.Column<int>(type: "int", nullable: false),
                    StrucutureTrauma = table.Column<int>(type: "int", nullable: false),
                    TechLevel = table.Column<int>(type: "int", nullable: false),
                    Slots = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Resources = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Caliber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Handling = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Distance = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: true),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CharacterAttributeModifier = table.Column<float>(type: "real", nullable: true),
                    Magazine = table.Column<int>(type: "int", nullable: false),
                    Encumbrance = table.Column<int>(type: "int", nullable: false),
                    TechLevel = table.Column<int>(type: "int", nullable: false),
                    Slots = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Resources = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeaponTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Weapons_WeaponTypes_WeaponTypeId",
                        column: x => x.WeaponTypeId,
                        principalTable: "WeaponTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DinarMoney = table.Column<int>(type: "int", nullable: false),
                    ChroniclerMoney = table.Column<int>(type: "int", nullable: false),
                    MaxEgo = table.Column<int>(type: "int", nullable: false),
                    Ego = table.Column<int>(type: "int", nullable: false),
                    CurrentSporeInfestation = table.Column<int>(type: "int", nullable: false),
                    MaxSporeInfestation = table.Column<int>(type: "int", nullable: false),
                    PermanentSporeInfestation = table.Column<int>(type: "int", nullable: false),
                    MaxFleshWounds = table.Column<int>(type: "int", nullable: false),
                    FleshWounds = table.Column<int>(type: "int", nullable: false),
                    MaxTrauma = table.Column<int>(type: "int", nullable: false),
                    Trauma = table.Column<int>(type: "int", nullable: false),
                    PassiveDefense = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CultureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConceptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdRoom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdApplicationUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_AspNetUsers_IdApplicationUser",
                        column: x => x.IdApplicationUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Concepts_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Cults_CultId",
                        column: x => x.CultId,
                        principalTable: "Cults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Rooms_IdRoom",
                        column: x => x.IdRoom,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptSkill",
                columns: table => new
                {
                    ConceptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptSkill", x => new { x.ConceptId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_ConceptSkill_Concepts_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concepts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConceptSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CultSkill",
                columns: table => new
                {
                    BonusSkillsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultSkill", x => new { x.BonusSkillsId, x.CultId });
                    table.ForeignKey(
                        name: "FK_CultSkill_Cults_CultId",
                        column: x => x.CultId,
                        principalTable: "Cults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultSkill_Skills_BonusSkillsId",
                        column: x => x.BonusSkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CultureSkill",
                columns: table => new
                {
                    BonusSkillsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CultureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultureSkill", x => new { x.BonusSkillsId, x.CultureId });
                    table.ForeignKey(
                        name: "FK_CultureSkill_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultureSkill_Skills_BonusSkillsId",
                        column: x => x.BonusSkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCSkills",
                columns: table => new
                {
                    NPCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCSkills", x => new { x.NPCId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_NPCSkills_NPCs_NPCId",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankPrerequisites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AttributeRequiredId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SkillRequiredId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SumRequired = table.Column<int>(type: "int", nullable: true),
                    BackgroundRequiredId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BackgroundLevelRequired = table.Column<int>(type: "int", nullable: true),
                    IsBackgroundPrerequisite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankPrerequisites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankPrerequisites_Attributes_AttributeRequiredId",
                        column: x => x.AttributeRequiredId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RankPrerequisites_Backgrounds_BackgroundRequiredId",
                        column: x => x.BackgroundRequiredId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RankPrerequisites_Skills_SkillRequiredId",
                        column: x => x.SkillRequiredId,
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NPCPotentials",
                columns: table => new
                {
                    NPCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PotentialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCPotentials", x => new { x.NPCId, x.PotentialId });
                    table.ForeignKey(
                        name: "FK_NPCPotentials_NPCs_NPCId",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCPotentials_Potentials_PotentialId",
                        column: x => x.PotentialId,
                        principalTable: "Potentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCEquipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NPCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsedSlots = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NPCEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCEquipments_NPCs_NPCId",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCWeapons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NPCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BulletsInMagazine = table.Column<int>(type: "int", nullable: false),
                    UsedSlots = table.Column<int>(type: "int", nullable: false),
                    SlotAttachments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCWeapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NPCWeapons_NPCs_NPCId",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCWeapons_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponWeaponQuality",
                columns: table => new
                {
                    QualitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponWeaponQuality", x => new { x.QualitiesId, x.WeaponId });
                    table.ForeignKey(
                        name: "FK_WeaponWeaponQuality_WeaponQualities_QualitiesId",
                        column: x => x.QualitiesId,
                        principalTable: "WeaponQualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeaponWeaponQuality_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterArtifacts",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtifactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChargeInMagazine = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterArtifacts", x => new { x.CharacterId, x.ArtifactId });
                    table.ForeignKey(
                        name: "FK_CharacterArtifacts_Artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterArtifacts_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAttributes",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAttributes", x => new { x.CharacterId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_CharacterAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterAttributes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CharacterBackgrounds",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackgroundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterBackgrounds", x => new { x.CharacterId, x.BackgroundId });
                    table.ForeignKey(
                        name: "FK_CharacterBackgrounds_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterBackgrounds_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterBurns",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BurnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterBurns", x => new { x.CharacterId, x.BurnId });
                    table.ForeignKey(
                        name: "FK_CharacterBurns_Burns_BurnId",
                        column: x => x.BurnId,
                        principalTable: "Burns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterBurns_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterEquipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsedSlots = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterEquipments_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterPotentials",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PotentialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPotentials", x => new { x.CharacterId, x.PotentialId });
                    table.ForeignKey(
                        name: "FK_CharacterPotentials_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterPotentials_Potentials_PotentialId",
                        column: x => x.PotentialId,
                        principalTable: "Potentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterProtections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProtectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsedConnectors = table.Column<int>(type: "int", nullable: false),
                    UsedSlots = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterProtections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterProtections_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterProtections_Protections_ProtectionId",
                        column: x => x.ProtectionId,
                        principalTable: "Protections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSkills",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSkills", x => new { x.CharacterId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_CharacterSkills_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CharacterVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsedSlots = table.Column<int>(type: "int", nullable: false),
                    FleshLost = table.Column<int>(type: "int", nullable: false),
                    TraumaLost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterVehicles_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterVehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterWeapons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BulletsInMagazine = table.Column<int>(type: "int", nullable: false),
                    UsedSlots = table.Column<int>(type: "int", nullable: false),
                    SlotAttachments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterWeapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterWeapons_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterWeapons_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankRankPrerequisite",
                columns: table => new
                {
                    PrerequisitesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankRankPrerequisite", x => new { x.PrerequisitesId, x.RankId });
                    table.ForeignKey(
                        name: "FK_RankRankPrerequisite_RankPrerequisites_PrerequisitesId",
                        column: x => x.PrerequisitesId,
                        principalTable: "RankPrerequisites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RankRankPrerequisite_Ranks_RankId",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CAttributeCulture_CultureId",
                table: "CAttributeCulture",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterArtifacts_ArtifactId",
                table: "CharacterArtifacts",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAttributes_AttributeId",
                table: "CharacterAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterBackgrounds_BackgroundId",
                table: "CharacterBackgrounds",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterBurns_BurnId",
                table: "CharacterBurns",
                column: "BurnId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterEquipments_CharacterId",
                table: "CharacterEquipments",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterEquipments_EquipmentId",
                table: "CharacterEquipments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPotentials_PotentialId",
                table: "CharacterPotentials",
                column: "PotentialId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProtections_CharacterId",
                table: "CharacterProtections",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProtections_ProtectionId",
                table: "CharacterProtections",
                column: "ProtectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ConceptId",
                table: "Characters",
                column: "ConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CultId",
                table: "Characters",
                column: "CultId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CultureId",
                table: "Characters",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IdApplicationUser",
                table: "Characters",
                column: "IdApplicationUser");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IdRoom",
                table: "Characters",
                column: "IdRoom");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkills_SkillId",
                table: "CharacterSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterVehicles_CharacterId",
                table: "CharacterVehicles",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterVehicles_VehicleId",
                table: "CharacterVehicles",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterWeapons_CharacterId",
                table: "CharacterWeapons",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterWeapons_WeaponId",
                table: "CharacterWeapons",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_BonusAttributeId",
                table: "Concepts",
                column: "BonusAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptSkill_SkillId",
                table: "ConceptSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_CultCulture_CultureId",
                table: "CultCulture",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_CultSkill_CultId",
                table: "CultSkill",
                column: "CultId");

            migrationBuilder.CreateIndex(
                name: "IX_CultureSkill_CultureId",
                table: "CultureSkill",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentTypeId",
                table: "Equipments",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCArtifacts_ArtifactId",
                table: "NPCArtifacts",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCArtifacts_NPCId",
                table: "NPCArtifacts",
                column: "NPCId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCAttributes_AttributeId",
                table: "NPCAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCBurns_BurnId",
                table: "NPCBurns",
                column: "BurnId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCEquipments_EquipmentId",
                table: "NPCEquipments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCEquipments_NPCId",
                table: "NPCEquipments",
                column: "NPCId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCPotentials_PotentialId",
                table: "NPCPotentials",
                column: "PotentialId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCProtections_NPCId",
                table: "NPCProtections",
                column: "NPCId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCProtections_ProtectionId",
                table: "NPCProtections",
                column: "ProtectionId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCSkills_SkillId",
                table: "NPCSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCWeapons_NPCId",
                table: "NPCWeapons",
                column: "NPCId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCWeapons_WeaponId",
                table: "NPCWeapons",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Potentials_CultId",
                table: "Potentials",
                column: "CultId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionProtectionQuality_QualitiesId",
                table: "ProtectionProtectionQuality",
                column: "QualitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_RankPrerequisites_AttributeRequiredId",
                table: "RankPrerequisites",
                column: "AttributeRequiredId");

            migrationBuilder.CreateIndex(
                name: "IX_RankPrerequisites_BackgroundRequiredId",
                table: "RankPrerequisites",
                column: "BackgroundRequiredId");

            migrationBuilder.CreateIndex(
                name: "IX_RankPrerequisites_SkillRequiredId",
                table: "RankPrerequisites",
                column: "SkillRequiredId");

            migrationBuilder.CreateIndex(
                name: "IX_RankRankPrerequisite_RankId",
                table: "RankRankPrerequisite",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_CultId",
                table: "Ranks",
                column: "CultId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CAttributeId",
                table: "Skills",
                column: "CAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_IdApplicationUser",
                table: "UserRooms",
                column: "IdApplicationUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_IdRoom",
                table: "UserRooms",
                column: "IdRoom");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_AttributeId",
                table: "Weapons",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_WeaponTypeId",
                table: "Weapons",
                column: "WeaponTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponWeaponQuality_WeaponId",
                table: "WeaponWeaponQuality",
                column: "WeaponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CAttributeCulture");

            migrationBuilder.DropTable(
                name: "CharacterArtifacts");

            migrationBuilder.DropTable(
                name: "CharacterAttributes");

            migrationBuilder.DropTable(
                name: "CharacterBackgrounds");

            migrationBuilder.DropTable(
                name: "CharacterBurns");

            migrationBuilder.DropTable(
                name: "CharacterEquipments");

            migrationBuilder.DropTable(
                name: "CharacterPotentials");

            migrationBuilder.DropTable(
                name: "CharacterProtections");

            migrationBuilder.DropTable(
                name: "CharacterSkills");

            migrationBuilder.DropTable(
                name: "CharacterVehicles");

            migrationBuilder.DropTable(
                name: "CharacterWeapons");

            migrationBuilder.DropTable(
                name: "ConceptSkill");

            migrationBuilder.DropTable(
                name: "CultCulture");

            migrationBuilder.DropTable(
                name: "CultSkill");

            migrationBuilder.DropTable(
                name: "CultureSkill");

            migrationBuilder.DropTable(
                name: "NPCArtifacts");

            migrationBuilder.DropTable(
                name: "NPCAttributes");

            migrationBuilder.DropTable(
                name: "NPCBurns");

            migrationBuilder.DropTable(
                name: "NPCEquipments");

            migrationBuilder.DropTable(
                name: "NPCPotentials");

            migrationBuilder.DropTable(
                name: "NPCProtections");

            migrationBuilder.DropTable(
                name: "NPCSkills");

            migrationBuilder.DropTable(
                name: "NPCWeapons");

            migrationBuilder.DropTable(
                name: "ProtectionProtectionQuality");

            migrationBuilder.DropTable(
                name: "RankRankPrerequisite");

            migrationBuilder.DropTable(
                name: "UserRooms");

            migrationBuilder.DropTable(
                name: "WeaponWeaponQuality");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Artifacts");

            migrationBuilder.DropTable(
                name: "Burns");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Potentials");

            migrationBuilder.DropTable(
                name: "NPCs");

            migrationBuilder.DropTable(
                name: "ProtectionQualities");

            migrationBuilder.DropTable(
                name: "Protections");

            migrationBuilder.DropTable(
                name: "RankPrerequisites");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropTable(
                name: "WeaponQualities");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Concepts");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "Backgrounds");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Cults");

            migrationBuilder.DropTable(
                name: "WeaponTypes");

            migrationBuilder.DropTable(
                name: "Attributes");
        }
    }
}
