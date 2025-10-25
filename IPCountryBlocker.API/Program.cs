using Hangfire;
using IPCountryBlocker.Application;
using IPCountryBlocker.Application.Middleware;
using IPCountryBlocker.Infrastructure;
using IPCountryBlocker.Service;
using IPCountryBlocker.Service.Implementations;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace IPCountryBlocker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddInfrastructureDependencies(builder.Configuration);
            builder.Services.AddServiceDependencies(builder.Configuration);
            builder.Services.AddCoreDependencies();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Localization
            builder.Services.AddControllersWithViews();
            builder.Services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "";
            });

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("de-DE"),
                new CultureInfo("fr-FR"),
                new CultureInfo("ar-EG")
            };

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
            });

            #endregion

            var app = builder.Build();

            var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire");

            RecurringJob.AddOrUpdate<TemporaryBlockCleaner>(
                "cleanup-temporary-blocks",
                job => job.RemoveExpiredBlocksAsync(),
                "*/5 * * * *",
                new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc }
                );

            app.MapControllers();

            app.Run();
        }
    }
}
