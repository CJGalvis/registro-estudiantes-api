using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudentsApi.Context;

var AllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Estudiantes", Version = "v1" }));
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(AllowSpecificOrigins);

app.UseAuthorization();
app.MapControllers();
app.Run();
