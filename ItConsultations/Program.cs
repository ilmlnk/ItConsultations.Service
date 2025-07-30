using ItConsultations.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddConsultationControllers()
    .AddDatabase(builder.Configuration)
    .AddConsultationConfiguration(builder.Configuration)
    .AddConsultationAuthentication(builder.Configuration)
    .AddRepositories()
    .AddBusinessServices()
    .AddFileServices(builder.Configuration)
    .AddValidationServices()
    .AddLoggingServices()
    .AddConsultationSwagger()
    .AddConsultationAutoMapper();

var app = builder.Build();

DatabaseInitializer.Initialize(app.Services);