using BackgroundService;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<BackgroundWorkerService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<BackgroundWorkerService>());
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/startCounter", ([FromServices] BackgroundWorkerService worker) =>
    {
        worker.StartNewTimer();
        var counterInfo = new CounterInfo(DateTime.Now, worker.TimerList.Count);
        return counterInfo;
    })
    .WithName("StartBackgroundCounter");

app.Run();