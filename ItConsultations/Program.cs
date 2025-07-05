using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.Services.ArticleService;
using ItConsultations.Business.Services.AttachmentService;
using ItConsultations.Business.Services.AuthService;
using ItConsultations.Business.Services.CoachService;
using ItConsultations.Business.Services.ConsultationService;
using ItConsultations.Business.Services.StudentService;
using ItConsultations.Business.Configs;
using ItConsultations.DataAccess.Repository;
using ItConsultations.DataAccess.Repository.EntityFramework;
using ItConsultations.Logger.Configs;
using ItConsultations.Logger.Services;
using ItConsultations.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<ConsultationsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<FirebaseConfig>(builder.Configuration.GetSection("Firebase"));

builder.Services.Configure<LogConfigs>(builder.Configuration.GetSection("Logging"));
builder.Services.AddSingleton<ILoggingService, LoggingService>();

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "your-super-secret-key-with-at-least-32-characters";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ItConsultations";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ItConsultationsUsers";

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
    });*/

builder.Services.AddAuthorization();

// Register repositories
//builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();

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
