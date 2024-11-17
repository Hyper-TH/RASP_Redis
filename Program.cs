using RASP_Redis.Models;
using RASP_Redis.Services;

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


// Takes direct dependency on MongoClient
builder.Services.AddSingleton<BooksService>();

builder.Services.AddControllers()
    .AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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
// app.UseSession(); // Use session middleware if using Redis for sessions

app.MapControllers();

app.Run();
