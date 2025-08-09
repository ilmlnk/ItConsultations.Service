using ItConsultations.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddConsultationControllers()
    .AddDatabase(builder.Configuration)
    .AddConsultationConfiguration(builder.Configuration)
    .AddConsultationAuthentication(builder.Configuration)
    .ConfigureDependencyInjection()
    .AddFileServices(builder.Configuration)
    .AddValidationServices()
    .AddLoggingServices()
    .AddConsultationSwagger()
    .AddConsultationAutoMapper();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Clear();
app.Urls.Add($"http://*:{port}");

DatabaseInitializer.Initialize(app.Services);
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.Run();