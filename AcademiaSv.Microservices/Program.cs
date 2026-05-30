using AcademiaSv.Microservices.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MicroservicesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS para que el proyecto principal pueda consumirlo
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAcademiaSv", policy =>
        policy.WithOrigins("https://localhost:7172", "http://localhost:5172")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAcademiaSv");
app.UseAuthorization();
app.MapControllers();
app.Run();
