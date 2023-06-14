using dotnet_webapi_with_caching.Data;
using dotnet_webapi_with_caching.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// add the database context
builder.Services.AddDbContext<ApiDbContext>(
    options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("DriversDB"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.16-mysql"));
    }
);
// inject the caching service 
builder.Services.AddScoped<ICachingService, CachingService>();

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

