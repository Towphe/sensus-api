using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SensusAPI.Models.DB;
using Microsoft.AspNetCore.Authentication.Certificate;

DotNetEnv.Env.Load(".env");
var builder = WebApplication.CreateBuilder(args);

// add services
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// builder.Services.AddHttpsRedirection(opts => {
//     opts.HttpsPort = 44350;
//     opts.RedirectStatusCode = 301;
// });
builder.Services.AddControllers();
builder.Services.AddDbContext<SensusDbContext>(opts => {
    opts.UseNpgsql(DotNetEnv.Env.GetString("PSQL_KEY"));
});
// builder.Services.AddAuthentication(
//         CertificateAuthenticationDefaults.AuthenticationScheme)
//     .AddCertificate();
builder.Services.AddScoped<IPollHandler, PollHandler>();    // poll handler

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

//app.UseHttpsRedirection();  // redirect requests to http

app.UseRouting(); // allow routing

app.MapControllers();   // map controllers

app.Run();