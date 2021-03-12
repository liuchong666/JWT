using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetPro.Swagger;
using Microsoft.AspNetCore.Authorization;
using JwtAuth.Authorization;
using MediatR;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using JwtAuth.FeatureManagement;

namespace JwtAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHostedService<MyFirstHostedService>();
            services.AddControllers();
            services.AddNetProSwagger(Configuration);
            services.AddMediatR(typeof(Startup));
            services.AddApiVersioning(v =>
            {
                v.ReportApiVersions = true;
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddFeatureManagement()
                .AddFeatureFilter<PercentageFilter>()
                .AddFeatureFilter<TimeWindowFilter>()
                .AddFeatureFilter<BrowserFeatureFilter>();

            services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("test", policy => policy.RequireClaim("id"));
                options.InvokeHandlersAfterFailure = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = "api",

                    ValidateIssuer = true,
                    ValidIssuer = "api",

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WeatherForecastControllerWeatherForecastController")),

                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30),
                };
            });

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiniProfiler();
            app.UseRouting();
            app.UseNetProSwagger();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
