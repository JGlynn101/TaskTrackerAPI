using TaskTrackerAPI.Repositories;
using TaskTrackerAPI.Services;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITaskService, TaskService>();
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// leave this off until you configure HTTPS ports/certs
// app.UseHttpsRedirection();

app.Run();


//HTTP request → TaskController (routes) → TaskService (business logic) → TaskRepository (JSON persistence)