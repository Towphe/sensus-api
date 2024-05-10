using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SensusAPI.Models.DB;

DotNetEnv.Env.Load(".env");
var builder = WebApplication.CreateBuilder(args);

// add services
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddHttpsRedirection(opts => {
    opts.HttpsPort = 44350;
    opts.RedirectStatusCode = 301;
});
builder.Services.AddControllers();
builder.Services.AddDbContext<SensusDbContext>(opts => {
    opts.UseNpgsql(DotNetEnv.Env.GetString("PSQL_KEY"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment()){
    // use CORS while application is in development
    app.UseCors();
}

app.UseHttpsRedirection();  // redirect requests to http

app.UseRouting(); // allow routing

app.MapControllers();   // map controllers

app.Run();