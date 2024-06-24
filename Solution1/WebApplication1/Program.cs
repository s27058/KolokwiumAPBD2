using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<KolokwiumContext>((opt =>
{
    opt.UseSqlServer("Name=ConnectionStrings:Default");
}));
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
        
var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseRouting();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();