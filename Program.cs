using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RASP_Redis.Models;
using RASP_Redis.Models.DatabaseSettings;
using RASP_Redis.Services.MongoDB;
using RASP_Redis.Services.Redis;
using StackExchange.Redis;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

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

// Databases
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.Configure<ProjectADatabaseSettings>(
    builder.Configuration.GetSection("ProjectADatabase"));

/* START BOOKSTORE DATABASE */
// Books Collection
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<BookStoreDatabaseSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString); 
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<Book>(settings.BooksCollectionName);
});
/* END BOOKSTORE DATABASE */


/* START PROJECTA DATABASE */
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProjectADatabaseSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString);
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<User>(settings.UsersCollectionName);
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProjectADatabaseSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString);
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<Meeting>(settings.MeetingsCollectionName);
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProjectADatabaseSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString);
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<Attendees>(settings.AttendeesCollectionName);
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProjectADatabaseSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString);
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<UserMeetings>(settings.UserMeetingsCollectionName);
});
/* END PROJECTA DATABASE */


builder.Services.AddControllers()
    .AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<MeetingsService>();
builder.Services.AddSingleton<AttendeesService>();
builder.Services.AddSingleton<UserMeetingsService>();

//builder.Services.AddSingleton<BookStoreRedisService>();
//builder.Services.AddSingleton<ProjectARedisService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();
// app.UseSession(); // Use session middleware if using Redis for sessions

app.MapControllers();

app.Run();
