using ItConsultations.Business.Configs;
using ItConsultations.Business.Services.ArticleService;
using ItConsultations.Business.Services.AttachmentService;
using ItConsultations.Business.Services.AuthService;
using ItConsultations.Business.Services.CoachService;
using ItConsultations.Business.Services.ConferenceService;
using ItConsultations.Business.Services.ConsultationService;
using ItConsultations.Business.Services.EventService;
using ItConsultations.Business.Services.FileService;
using ItConsultations.Business.Services.GoogleCalendarService;
using ItConsultations.Business.Services.NoteService;
using ItConsultations.Business.Services.StudentService;
using ItConsultations.Converters;
using ItConsultations.DataAccess.FileAccess;
using ItConsultations.DataAccess.Interfaces;
using ItConsultations.DataAccess.Repository;
using ItConsultations.DataAccess.Repository.EntityFramework;
using ItConsultations.Logger.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ItConsultations.Configuration;

public static class ServiceConfigurations
{
    public static IServiceCollection AddConsultationControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ConsultationsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection AddConsultationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FirebaseConfig>(configuration.GetSection("Firebase"));

        services.Configure<LogConfigs>(configuration.GetSection("Logging"));
        services.AddSingleton<LogConfigs>(provider =>
        {
            var config = new LogConfigs();
            configuration.GetSection("Logging").Bind(config);
            return config;
        });

        return services;
    }

    public static IServiceCollection AddConsultationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSecret = configuration["Jwt:Secret"];
        var jwtIssuer = configuration["Jwt:Issuer"];
        var jwtAudience = configuration["Jwt:Audience"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret))
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();
        services.AddScoped<ICoachService, CoachService>();
        services.AddScoped<IConsultationService, ConsultationService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IConferenceService, ConferenceService>();

        return services;
    }

    public static IServiceCollection AddFileServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped(typeof(IFileStorage), provider =>
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var containerName = configuration["AzureStorage:ContainerName"] ?? "itconsultations";

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Azure Storage connection string is not configured");
            }

            return new AzureBlobStorage(connectionString, containerName);
        });

        return services;
    }

    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        return null;
    }

    public static IServiceCollection AddLoggingServices(this IServiceCollection services)
    {
        return null;
    }

    public static IServiceCollection AddConsultationSwagger(this IServiceCollection services)
    {
        return null;
    }

    public static IServiceCollection AddConsultationAutoMapper(this IServiceCollection services)
    {
        return null;
    }
}
