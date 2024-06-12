using Crayon.API.Contracts;
using Crayon.API.Models.Database;
using Crayon.API.Repositories;
using Crayon.API.Services;
using Crayon.API.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICCPRepository, CCPRepository>();
builder.Services.AddScoped<IRedisRepository, RedisRepository>();
builder.Services.AddScoped<ICrayonService, CrayonService>();


var databaseSettings = builder.Configuration.GetSection(DatabaseSettings.DatabaseSettingsSettingsName).Get<DatabaseSettings>();
builder.Services.AddDbContext<CrayonDbContext>(options => options.UseSqlServer(databaseSettings.ConnectionString), ServiceLifetime.Transient);


var redisSettings = builder.Configuration.GetSection(RedisSettings.RedisSettingsName).Get<RedisSettings>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisSettings.ConnectionString));

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(RedisSettings.RedisSettingsName));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(DatabaseSettings.DatabaseSettingsSettingsName));
builder.Services.Configure<CCPSettings>(builder.Configuration.GetSection(CCPSettings.CCPSettingsName));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


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
