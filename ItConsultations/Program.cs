using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.Services.AuthService;
using ItConsultations.Business.Services.ArticleService;
using ItConsultations.Business.Services.AttachmentService;
using ItConsultations.Business.Services.CoachService;
using ItConsultations.Business.Services.ConsultationService;
using ItConsultations.Business.Services.StudentService;
using ItConsultations.Business.Services.Validation;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Configs;
using ItConsultations.Business.Services.Validation.Access.Articles;
using ItConsultations.Business.Services.Validation.Access.Consultations;
using ItConsultations.Business.Services.Validation.Access.Attachments;
using ItConsultations.Business.Services.Validation.Access.Coaches;
using ItConsultations.Business.Services.Validation.Access.Students;
using ItConsultations.Business.Services.Validation.Consultation;
using ItConsultations.Business.Services.Validation.Student;
using ItConsultations.Business.Services.Validation.Coach;
using ItConsultations.DataAccess.Repository;
using ItConsultations.DataAccess.Repository.EntityFramework;
using ItConsultations.Logger.Configs;
using ItConsultations.Logger.Services;
using ItConsultations.Middleware;
using Microsoft.EntityFrameworkCore;
using ItConsultations.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<ConsultationsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), 
    ServiceLifetime.Transient);

builder.Services.Configure<FirebaseConfig>(builder.Configuration.GetSection("Firebase"));

// Configure LogConfigs
builder.Services.Configure<LogConfigs>(builder.Configuration.GetSection("Logging"));
builder.Services.AddSingleton<LogConfigs>(provider =>
{
    var config = new LogConfigs();
    builder.Configuration.GetSection("Logging").Bind(config);
    return config;
});
builder.Services.AddSingleton<ILoggingService, LoggingService>();

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "your-super-secret-key-with-at-least-32-characters";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ItConsultations";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ItConsultationsUsers";

builder.Services.AddAuthorization();

// Register repositories
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();

// Register validation services
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<ICoachValidationService, CoachValidationService>();
builder.Services.AddScoped<IConsultationValidationService, ConsultationValidationService>();
builder.Services.AddScoped<IStudentValidationService, StudentValidationService>();

// Register access validation services
builder.Services.AddScoped<ICoachAccessValidationService, CoachAccessValidationService>();
builder.Services.AddScoped<IConsultationAccessValidationService, ConsultationAccessValidationService>();
builder.Services.AddScoped<IStudentAccessValidationService, StudentAccessValidationService>();
builder.Services.AddScoped<IArticleAccessValidationService, ArticleAccessValidationService>();
builder.Services.AddScoped<IAttachmentAccessValidationService, AttachmentAccessValidationService>();

MapperManager.Initialize(cfg =>
{
    cfg.AddProfile<ConsultationsAutoMapperProfile>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ConsultationsDbContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthLogging();

app.MapControllers();

app.Run();
