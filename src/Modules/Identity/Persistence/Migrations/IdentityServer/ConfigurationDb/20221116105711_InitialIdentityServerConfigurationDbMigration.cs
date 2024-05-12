#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Astrum.Identity.Migrations.IdentityServer.ConfigurationDb;

public partial class InitialIdentityServerConfigurationDbMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "Identity_Configuration");

        migrationBuilder.CreateTable(
            "ApiResources",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                AllowedAccessTokenSigningAlgorithms =
                    table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                RequireResourceIndicator = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "ApiScopes",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Required = table.Column<bool>("boolean", nullable: false),
                Emphasize = table.Column<bool>("boolean", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Clients",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                ClientId = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ProtocolType = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                RequireClientSecret = table.Column<bool>("boolean", nullable: false),
                ClientName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                ClientUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                LogoUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                RequireConsent = table.Column<bool>("boolean", nullable: false),
                AllowRememberConsent = table.Column<bool>("boolean", nullable: false),
                AlwaysIncludeUserClaimsInIdToken = table.Column<bool>("boolean", nullable: false),
                RequirePkce = table.Column<bool>("boolean", nullable: false),
                AllowPlainTextPkce = table.Column<bool>("boolean", nullable: false),
                RequireRequestObject = table.Column<bool>("boolean", nullable: false),
                AllowAccessTokensViaBrowser = table.Column<bool>("boolean", nullable: false),
                FrontChannelLogoutUri =
                    table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                FrontChannelLogoutSessionRequired = table.Column<bool>("boolean", nullable: false),
                BackChannelLogoutUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                BackChannelLogoutSessionRequired = table.Column<bool>("boolean", nullable: false),
                AllowOfflineAccess = table.Column<bool>("boolean", nullable: false),
                IdentityTokenLifetime = table.Column<int>("integer", nullable: false),
                AllowedIdentityTokenSigningAlgorithms =
                    table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                AccessTokenLifetime = table.Column<int>("integer", nullable: false),
                AuthorizationCodeLifetime = table.Column<int>("integer", nullable: false),
                ConsentLifetime = table.Column<int>("integer", nullable: true),
                AbsoluteRefreshTokenLifetime = table.Column<int>("integer", nullable: false),
                SlidingRefreshTokenLifetime = table.Column<int>("integer", nullable: false),
                RefreshTokenUsage = table.Column<int>("integer", nullable: false),
                UpdateAccessTokenClaimsOnRefresh = table.Column<bool>("boolean", nullable: false),
                RefreshTokenExpiration = table.Column<int>("integer", nullable: false),
                AccessTokenType = table.Column<int>("integer", nullable: false),
                EnableLocalLogin = table.Column<bool>("boolean", nullable: false),
                IncludeJwtId = table.Column<bool>("boolean", nullable: false),
                AlwaysSendClientClaims = table.Column<bool>("boolean", nullable: false),
                ClientClaimsPrefix = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                PairWiseSubjectSalt = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                UserSsoLifetime = table.Column<int>("integer", nullable: true),
                UserCodeType = table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                DeviceCodeLifetime = table.Column<int>("integer", nullable: false),
                CibaLifetime = table.Column<int>("integer", nullable: true),
                PollingInterval = table.Column<int>("integer", nullable: true),
                CoordinateLifetimeWithUserSession = table.Column<bool>("boolean", nullable: true),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "IdentityProviders",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scheme = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Type = table.Column<string>("character varying(20)", maxLength: 20, nullable: false),
                Properties = table.Column<string>("text", nullable: true),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityProviders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "IdentityResources",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Required = table.Column<bool>("boolean", nullable: false),
                Emphasize = table.Column<bool>("boolean", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "ApiResourceClaims",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceClaims_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceProperties",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceProperties_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceScopes",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scope = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ApiResourceId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceScopes_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceSecrets",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Value = table.Column<string>("character varying(4000)", maxLength: 4000, nullable: false),
                Expiration = table.Column<DateTime>("timestamp with time zone", nullable: true),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceSecrets_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiScopeClaims",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ScopeId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ApiScopeClaims_ApiScopes_ScopeId",
                    x => x.ScopeId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiScopeProperties",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ScopeId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ApiScopeProperties_ApiScopes_ScopeId",
                    x => x.ScopeId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientClaims",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ClientClaims_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientCorsOrigins",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Origin = table.Column<string>("character varying(150)", maxLength: 150, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
                table.ForeignKey(
                    "FK_ClientCorsOrigins_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientGrantTypes",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                GrantType = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
                table.ForeignKey(
                    "FK_ClientGrantTypes_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientIdPRestrictions",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Provider = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
                table.ForeignKey(
                    "FK_ClientIdPRestrictions_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientPostLogoutRedirectUris",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PostLogoutRedirectUri = table.Column<string>("character varying(400)", maxLength: 400, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
                table.ForeignKey(
                    "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientProperties",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ClientProperties_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientRedirectUris",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RedirectUri = table.Column<string>("character varying(400)", maxLength: 400, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
                table.ForeignKey(
                    "FK_ClientRedirectUris_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientScopes",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scope = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientScopes", x => x.Id);
                table.ForeignKey(
                    "FK_ClientScopes_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientSecrets",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>("integer", nullable: false),
                Description = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                Value = table.Column<string>("character varying(4000)", maxLength: 4000, nullable: false),
                Expiration = table.Column<DateTime>("timestamp with time zone", nullable: true),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                table.ForeignKey(
                    "FK_ClientSecrets_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "IdentityResourceClaims",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdentityResourceId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
                table.ForeignKey(
                    "FK_IdentityResourceClaims_IdentityResources_IdentityResourceId",
                    x => x.IdentityResourceId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "IdentityResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "IdentityResourceProperties",
            schema: "Identity_Configuration",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdentityResourceId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
                table.ForeignKey(
                    "FK_IdentityResourceProperties_IdentityResources_IdentityResour~",
                    x => x.IdentityResourceId,
                    principalSchema: "Identity_Configuration",
                    principalTable: "IdentityResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_ApiResourceClaims_ApiResourceId_Type",
            schema: "Identity_Configuration",
            table: "ApiResourceClaims",
            columns: new[] {"ApiResourceId", "Type"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceProperties_ApiResourceId_Key",
            schema: "Identity_Configuration",
            table: "ApiResourceProperties",
            columns: new[] {"ApiResourceId", "Key"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResources_Name",
            schema: "Identity_Configuration",
            table: "ApiResources",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceScopes_ApiResourceId_Scope",
            schema: "Identity_Configuration",
            table: "ApiResourceScopes",
            columns: new[] {"ApiResourceId", "Scope"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceSecrets_ApiResourceId",
            schema: "Identity_Configuration",
            table: "ApiResourceSecrets",
            column: "ApiResourceId");

        migrationBuilder.CreateIndex(
            "IX_ApiScopeClaims_ScopeId_Type",
            schema: "Identity_Configuration",
            table: "ApiScopeClaims",
            columns: new[] {"ScopeId", "Type"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiScopeProperties_ScopeId_Key",
            schema: "Identity_Configuration",
            table: "ApiScopeProperties",
            columns: new[] {"ScopeId", "Key"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiScopes_Name",
            schema: "Identity_Configuration",
            table: "ApiScopes",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientClaims_ClientId_Type_Value",
            schema: "Identity_Configuration",
            table: "ClientClaims",
            columns: new[] {"ClientId", "Type", "Value"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientCorsOrigins_ClientId_Origin",
            schema: "Identity_Configuration",
            table: "ClientCorsOrigins",
            columns: new[] {"ClientId", "Origin"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientGrantTypes_ClientId_GrantType",
            schema: "Identity_Configuration",
            table: "ClientGrantTypes",
            columns: new[] {"ClientId", "GrantType"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientIdPRestrictions_ClientId_Provider",
            schema: "Identity_Configuration",
            table: "ClientIdPRestrictions",
            columns: new[] {"ClientId", "Provider"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri",
            schema: "Identity_Configuration",
            table: "ClientPostLogoutRedirectUris",
            columns: new[] {"ClientId", "PostLogoutRedirectUri"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientProperties_ClientId_Key",
            schema: "Identity_Configuration",
            table: "ClientProperties",
            columns: new[] {"ClientId", "Key"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientRedirectUris_ClientId_RedirectUri",
            schema: "Identity_Configuration",
            table: "ClientRedirectUris",
            columns: new[] {"ClientId", "RedirectUri"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Clients_ClientId",
            schema: "Identity_Configuration",
            table: "Clients",
            column: "ClientId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientScopes_ClientId_Scope",
            schema: "Identity_Configuration",
            table: "ClientScopes",
            columns: new[] {"ClientId", "Scope"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientSecrets_ClientId",
            schema: "Identity_Configuration",
            table: "ClientSecrets",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            "IX_IdentityProviders_Scheme",
            schema: "Identity_Configuration",
            table: "IdentityProviders",
            column: "Scheme",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResourceClaims_IdentityResourceId_Type",
            schema: "Identity_Configuration",
            table: "IdentityResourceClaims",
            columns: new[] {"IdentityResourceId", "Type"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResourceProperties_IdentityResourceId_Key",
            schema: "Identity_Configuration",
            table: "IdentityResourceProperties",
            columns: new[] {"IdentityResourceId", "Key"},
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResources_Name",
            schema: "Identity_Configuration",
            table: "IdentityResources",
            column: "Name",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "ApiResourceClaims",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ApiResourceProperties",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ApiResourceScopes",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ApiResourceSecrets",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ApiScopeClaims",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ApiScopeProperties",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientClaims",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientCorsOrigins",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientGrantTypes",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientIdPRestrictions",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientPostLogoutRedirectUris",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientProperties",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientRedirectUris",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientScopes",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ClientSecrets",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "IdentityProviders",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "IdentityResourceClaims",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "IdentityResourceProperties",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ApiResources",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "ApiScopes",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "Clients",
            "Identity_Configuration");

        migrationBuilder.DropTable(
            "IdentityResources",
            "Identity_Configuration");
    }
}