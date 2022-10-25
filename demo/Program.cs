using demo.Models;
using Demo.BUS.BUS;
using Demo.BUS.IBUS;
using Demo.Helper.AutoMapperProfiles;
using Demo.Repository.IRepository;
using Demo.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Demo.Helper.FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<DemoDbContext>(option =>
    option.UseMySql("server=localhost;user id=root;password='';port=3306;database=demo;",
    ServerVersion.AutoDetect("server=localhost;user id=root;password='';port=3306;database=demo;")));

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserBUS, UserBUS>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
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