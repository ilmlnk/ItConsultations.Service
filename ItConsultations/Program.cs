using ItConsultations.Infrastructure;
using ItConsultations.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureHosting();

builder.Services
    .AddConsultationControllers()
    .AddDatabase(builder.Configuration)
    .AddConsultationCors()
    .AddConsultationConfiguration(builder.Configuration)
    .AddConsultationAuthentication(builder.Configuration)
    .ConfigureDependencyInjection()
    .AddFileServices(builder.Configuration)
    .AddValidationServices()
    .AddLoggingServices()
    .AddConsultationSwagger()
    .AddConsultationAutoMapper();

var app = builder.Build();

DatabaseInitializer.Initialize(app.Services);

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();