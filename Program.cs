using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RASP_Redis.Models;
using RASP_Redis.Models.Auth;
using RASP_Redis.Models.DatabaseSettings;
using RASP_Redis.Models.ProjectA;
using RASP_Redis.Services.MongoDB;
using RASP_Redis.Services.MongoDB.Utils;
using RASP_Redis.Services.Redis;
using StackExchange.Redis;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


// -------------------
// Redis Configuration
// -------------------
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConfig = builder.Configuration["Redis:BookStoreInstance:ConnectionString"];
    return ConnectionMultiplexer.Connect(redisConfig);
}).AddSingleton<BookStoreRedisService>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConfig = builder.Configuration["Redis:ProjectAInstance:ConnectionString"];
    return ConnectionMultiplexer.Connect(redisConfig);
}).AddSingleton<ProjectARedisService>();


// -----------------------------
// Register MongoDB Dependencies
// -----------------------------
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var projectADbSettings = builder.Configuration.GetSection("ProjectADatabase").Get<ProjectADatabaseSettings>();
    if (string.IsNullOrEmpty(projectADbSettings.ConnectionString))
    {
        throw new Exception("ProjectA Database ConnectionString is not configured.");
    }

    return new MongoClient(projectADbSettings.ConnectionString);
});


// Register IUserService and ISessionService
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ISessionService, SessionService>();

// -----------------
// JWT Authentication
// -----------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY"); ;

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new Exception("JWT SecretKey is not configured.");
        }

        if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
        {
            throw new Exception("JWT SecretKey must be at least 32 characters long.");
        }


        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// -----------------------------
// Configure Database Settings
// -----------------------------
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.Configure<ProjectADatabaseSettings>(
    builder.Configuration.GetSection("ProjectADatabase"));

// Register MongoDB collections
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<BookStoreDatabaseSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<Book>(settings.BooksCollectionName);
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProjectADatabaseSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<User>(settings.UsersCollectionName);
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProjectADatabaseSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<Meeting>(settings.MeetingsCollectionName);
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProjectADatabaseSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<Attendees>(settings.AttendeesCollectionName);
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProjectADatabaseSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<UserMeetings>(settings.UserMeetingsCollectionName);
});

// -----------------
// Register Services
// -----------------
builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<MeetingsService>();
builder.Services.AddSingleton<AttendeesService>();
builder.Services.AddSingleton<UserMeetingsService>();
builder.Services.AddSingleton<UnregisterUsers>();

// ------------------
// Add CORS and Swagger
// ------------------
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// -----------------
// Configure Middleware
// -----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

// Ensure Authentication Middleware is added before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();