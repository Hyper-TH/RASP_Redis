using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RASP_Redis.Models;
using RASP_Redis.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});

// Optional: Set up Redis-backed sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));


// Register Books collection
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<BookStoreDatabaseSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString); // Creates the client here
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<Book>(settings.BooksCollectionName);
});

// Register ISBNs collection
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<BookStoreDatabaseSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString); // Creates the client here
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<ISBN>(settings.ISBNsCollectionName);
});

builder.Services.AddControllers()
    .AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
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
builder.Services.AddSingleton<ISBNsService>();

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
