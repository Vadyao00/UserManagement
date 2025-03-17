using System.Text.Json.Serialization;
using Controllers;
using Controllers.Filters;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using UserManagement.API.Converters;
using UserManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services,builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

ConfigureApp(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    services.ConfigureCors();

    services.AddScoped<ValidationFilterAttribute>();

    services.ConfigureLoggerService();

    services.ConfigureRepositoryManager();

    services.ConfigureServiceManager();

    services.ConfigureSqlContext(configuration);

    services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
            config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
        }).AddXmlDataContractSerializerFormatters()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        })
        .AddApplicationPart(typeof(AssemblyReference).Assembly);

    // services.AddMediatR(cfg =>
    //     cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));
    services.AddAutoMapper(typeof(Program));

    services.AddAuthentication();
    services.ConfigureJWT(configuration);
    services.AddJwtConfiguration(configuration);
    services.AddAuthorization();
    services.AddRazorPages();
}

static void ConfigureApp(IApplicationBuilder app)
{
    app.UseHttpsRedirection();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
    });
    app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

}

static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>().First();