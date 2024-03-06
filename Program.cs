using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using PeopleManagmentSystem_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PeopleManagmentSystem_API.Services.Interfaces;
using MongoDB.Bson;
using Microsoft.OpenApi.Models;
using PeopleManagmentSystem_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
builder.Services.Configure<PeopleManagmentDatabaseSettings>(
                builder.Configuration.GetSection(nameof(PeopleManagmentDatabaseSettings)));

builder.Services.AddSingleton<IPeopleManagmentDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<PeopleManagmentDatabaseSettings>>().Value);

var settings = MongoClientSettings.FromConnectionString(builder.Configuration.GetValue<string>("PeopleManagmentDatabaseSettings:ConnectionString"));
settings.ApplicationName = "people_managment";
settings.UseTls = true;
settings.SslSettings = new SslSettings()
{
    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12,
};

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(settings));

var mongoDbIdentityConfig = new MongoDbIdentityConfiguration
{
    MongoDbSettings = new MongoDbSettings
    {
        ConnectionString = builder.Configuration.GetValue<string>("PeopleManagmentDatabaseSettings:ConnectionString"),
        DatabaseName = builder.Configuration.GetValue<string>("PeopleManagmentDatabaseSettings:DatabaseName")
    },
    IdentityOptionsAction = options =>
    {
        options.Password.RequiredLength = 8;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(10);
        options.Lockout.MaxFailedAccessAttempts = 5;

       options.User.RequireUniqueEmail = true;
    }
};

builder.Services.ConfigureMongoDbIdentity<User, Role, ObjectId>(mongoDbIdentityConfig)
    .AddUserManager<UserManager<User>>()
    .AddSignInManager<SignInManager<User>>()
    .AddRoleManager<RoleManager<Role>>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
    x.Authority = AuthOptions.ISSUER;
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.IncludeErrorDetails = true;
    x.Configuration = new OpenIdConnectConfiguration();
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthOptions.AUDIENCE,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ClockSkew = TimeSpan.Zero
};
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyUserService, CompanyUserService>();

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PeopleManagmentSystem-API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true)
               .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
