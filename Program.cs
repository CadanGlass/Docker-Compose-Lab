using Microsoft.EntityFrameworkCore;
using StudentsMinimalApi.Data;
using StudentsMinimalApi.Models;
using StudentsMinimalApi.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseNpgsql(connStr));
// Add Cors
builder.Services.AddCors(o => o.AddPolicy("Policy", builder => {
  builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

var app = builder.Build(); // ^^^^^^^^^^%^%^%^

app.UseCors("Policy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var route = app.MapGroup("/api/students");

route.MapGet("/", StudentService.GetAllStudents);
route.MapGet("/school/{school}", StudentService.GetStudentsBySchool);
route.MapGet("/{id}", StudentService.GetStudent);
route.MapPost("/", StudentService.CreateSttudent);
route.MapPut("/{id}", StudentService.UpdateStudent);
route.MapDelete("/{id}", StudentService.DeleteStudent);

// eqivalent to dotnet ef database update
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SchoolDbContext>();    
    context.Database.Migrate();
}

app.Run();

