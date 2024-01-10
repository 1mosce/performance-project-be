using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<PeopleManagmentDatabaseSettings>(
                builder.Configuration.GetSection(nameof(PeopleManagmentDatabaseSettings)));

builder.Services.AddSingleton<IPeopleManagmentDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<PeopleManagmentDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("PeopleManagmentDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<ICompanyService, CompanyService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
