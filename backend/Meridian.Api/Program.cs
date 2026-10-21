using Meridian.Api.Data;
using Meridian.Api.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<DbSeed>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<MemberStatesService>();
builder.Services.AddScoped<ImportService>();
builder.Services.AddScoped<ImportProcessor>();
builder.Services.AddScoped<VoteService>();
builder.Services.AddScoped<ConflictService>();
builder.Services.AddDbContext<MeridianContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MeridianContext>();
    await db.Database.MigrateAsync();

    // Load users and member states if the tables are empty
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeed>();
    await seeder.SeedAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
