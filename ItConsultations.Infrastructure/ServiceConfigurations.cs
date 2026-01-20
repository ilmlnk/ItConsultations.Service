using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using ItConsultations.Business.Services.ArticleService;
using Microsoft.Extensions.Configuration;
using ItConsultations.Business.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Services.AttachmentService;
using ItConsultations.Business.Services.AuthService;
using ItConsultations.Business.Services.CoachService;
using ItConsultations.Business.Services.ConsultationService;
using ItConsultations.Business.Services.StudentsListService;
using ItConsultations.Business.Services.StudentService;
using ItConsultations.Business.Services.EventService;
using ItConsultations.Business.Services.GoogleCalendarService;
using ItConsultations.Business.Services.NoteService;
using ItConsultations.Business.Services.ConferenceService;
using ItConsultations.Business.Services.FileService;
using ItConsultations.Business.Services.UserService;
using ItConsultations.Business.Services.Validation;
using ItConsultations.Business.Services.AccessValidation.CoachValidation;
using ItConsultations.Business.Services.AccessValidation.ConsultationValidation;
using ItConsultations.Business.Services.Validation.AccessValidation.Consultations;
using ItConsultations.Business.Services.Validation.AccessValidation.Students;
using ItConsultations.Business.Services.Validation.AccessValidation.Articles;
using ItConsultations.Business.Services.Validation.AccessValidation.Attachments;
using ItConsultations.Business.Services.AccessValidation.StudentValidation;
using Microsoft.OpenApi.Models;
using ItConsultations.Business.AutoMapperConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using ItConsultations.DataAccess.Repository.EntityFramework;
using ItConsultations.Logger.Configs;
using ItConsultations.DataAccess.Repository;
using ItConsultations.DataAccess.FileAccess;
using ItConsultations.Business.Services.Validation.AccessValidation.Coaches;
using ItConsultations.Logger.Services;
using ItConsultations.Business.Services.MetaService;
using ItConsultations.Logger.Providers;

namespace ItConsultations.Infrastructure;

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
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ConsultationsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("ItConsultations.Database")),
            ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection AddConsultationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FirebaseConfig>(configuration.GetSection("Firebase"));

        services.Configure<LogConfigs>(configuration.GetSection("Logging"));
        services.AddSingleton(provider =>
        {
            var config = new LogConfigs();
            configuration.GetSection("Logging").Bind(config);
            return config;
        });

        return services;
    }

    public static IServiceCollection AddConsultationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var firebaseProjectId = configuration["Firebase:ProjectId"];
        var authority = $"https://securetoken.google.com/{firebaseProjectId}";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = true,
                    ValidAudience = firebaseProjectId,
                    ValidateLifetime = true,
                };
            });

        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();
        services.AddScoped<ICoachService, CoachService>();
        services.AddScoped<IConsultationService, ConsultationService>();
        //services.AddScoped<IDocumentFormattingService, DocumentFormattingService>();
        services.AddScoped<IStudentsListService, StudentsListService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IConferenceService, ConferenceService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMetaService, MetaService>();
        
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
        // entity validation
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ICoachValidationService, CoachValidationService>();
        services.AddScoped<IConsultationValidationService, ConsultationValidationService>();
        services.AddScoped<IStudentValidationService, StudentValidationService>();
        // access validation
        services.AddScoped<ICoachAccessValidationService, CoachAccessValidationService>();
        services.AddScoped<IConsultationAccessValidationService, ConsultationAccessValidationService>();
        services.AddScoped<IStudentAccessValidationService, StudentAccessValidationService>();
        services.AddScoped<IArticleAccessValidationService, ArticleAccessValidationService>();
        services.AddScoped<IAttachmentAccessValidationService, AttachmentAccessValidationService>();

        return services;
    }

    public static IServiceCollection AddLoggingServices(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseLoggerProvider, DatabaseLoggerProvider>();
        services.AddSingleton<ILoggingService, LoggingService>();
        return services;
    }

    public static IServiceCollection AddConsultationSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "WaveIT API",
                Version = "v1",
                Description = @"
                    # WaveIT API
                    
                    - Client management
                    - Booking consultations 
                    - Consultation management
                    - Billing and transactions processing
                    - Reports and analytics
                ",
                Contact = new OpenApiContact
                {
                    Name = "Developers team",
                    Email = "support@waveit.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                },
                TermsOfService = new Uri("https://waveit.dev/terms")
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddConsultationAutoMapper(this IServiceCollection services)
    {
        MapperManager.Initialize(cfg =>
        {
            cfg.AddProfile<ConsultationsAutoMapperProfile>();
        });

        return services;
    }

    public static WebApplicationBuilder ConfigureHosting(this WebApplicationBuilder builder)
    {
        var port = Environment.GetEnvironmentVariable("PORT") 
            ?? builder.Configuration["ServerSettings:Port"] 
            ?? "5000";
        builder.WebHost.UseUrls($"http://*:{port}");
        return builder;
    }

    public static IServiceCollection AddConsultationCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        return services;
    }
}
