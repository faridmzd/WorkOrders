using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using W.O.API.Data;
using W.O.API.Data.Repositories.Abstract;
using W.O.API.Data.Repositories.Concrete;
using W.O.API.ExceptionsHandling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["SQLServerConnection"]));

builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IVisitRepository, VisitRepository>();
builder.Services.AddScoped<IPartRepository, PartRepository>();



var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
{
    policy.WithOrigins("https://localhost:7166", "http://localhost:5293")
    .AllowAnyMethod()
    .AllowAnyHeader();
}
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleWare>();

app.MapControllers();

app.Run();
