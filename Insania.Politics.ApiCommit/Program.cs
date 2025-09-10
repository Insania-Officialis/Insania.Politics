using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using NetTopologySuite;
using NetTopologySuite.IO.Converters;
using Serilog;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Middleware;
using Insania.Shared.Messages;
using Insania.Shared.Services;

using Insania.Politics.BusinessLogic;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Middleware;
using Insania.Politics.Models.Mapper;

//�������� ���������� ��������� ���-����������
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//��������� �������� ���-����������
IServiceCollection services = builder.Services;

//��������� ������������ ���-����������
IConfiguration configuration = builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
#if DEBUG
    .AddJsonFile("appsettings.Development.json", true, false)
#else
    .AddJsonFile("appsettings.Production.json", true, false)
#endif
    .Build();

//�������� ���������� ��� ������
var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["TokenSettings:Key"]!));
var issuer = configuration["TokenSettings:Issuer"];
var audience = configuration["TokenSettings:Audience"];

//���������� ���������� �����������
services
    .AddAuthorizationBuilder()
    .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes("Bearer")
    .RequireAuthenticatedUser().Build());

//���������� ���������� ��������������
services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = issuer,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = audience,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = key,
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });

//��������� ������������ ��������
services.AddSingleton(_ => configuration); //������������
services.AddScoped<ITransliterationSL, TransliterationSL>(); //������ ��������������
services.AddScoped<IPolygonParserSL, PolygonParserSL>(); //������ �������������� ��������
services.AddPoliticsBL(); //������� ������ � ������-������� � ���� ���������

//���������� ���������� �� � ��������� ��������
services.AddDbContext<PoliticsContext>(options =>
{
    string connectionString = configuration.GetConnectionString("Politics") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
    options.UseNpgsql(
        connectionString,
        x => x.UseNetTopologySuite()
    );
}); //�� ���������
services.AddDbContext<LogsApiPoliticsContext>(options =>
{
    string connectionString = configuration.GetConnectionString("LogsApiPolitics") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
    options.UseNpgsql(connectionString);
}); //�� ����� api � ���� ���������

//��������� ������������� ����� ���� � �������
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//���������� ���������� ������������ � �������������� json
services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

//���������� ���������� �����������
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
    .WriteTo.Debug()
    .CreateLogger();
services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

//���������� ���������� ������������
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Insania API", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Insania.Politics.ApiCommit.xml");
    options.IncludeXmlComments(filePath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "����������� �� ����� ����������",
        Scheme = "Bearer"
    });
    options.OperationFilter<AuthenticationRequirementsOperationFilter>();
});

//���������� ������
services.AddCors(options =>
{
    options.AddPolicy("BadPolicy", policyBuilder => policyBuilder
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
    );

    options.DefaultPolicyName = "CorsPolicy";
});

//���������� ����������� ������ � ������������
var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

//���������� ������������
services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new GeoJsonConverterFactory(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326)));
    });

//���������� ���������� �������������� �������
services.AddAutoMapper(cfg => cfg.AddProfile<PoliticsMappingProfile>());

//����������� ������ ���������� �����������
builder.Services.AddSingleton<List<string>>(
[

]);

//���������� ���-����������
WebApplication app = builder.Build();

//����������� �������������
app.UseRouting();

//����������� ��������������
app.UseAuthentication();

//����������� �����������
app.UseAuthorization();

//���������� ��������� ��������
app.UseMiddleware<LoggingMiddleware>(); //������������
app.UseMiddleware<Insania.Shared.Middleware.AuthorizationMiddleware>(); //�����������

//����������� ��������
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Insania API V1");
});

//����������� ������
app.UseCors("CorsPolicy");

//����������� ������������� ������������
app.MapControllers();

//������ ���-����������
app.Run();