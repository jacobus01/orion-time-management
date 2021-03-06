using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orion.DAL.EF.Models.DB;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.AccessTokenValidation;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Orion.Web.API.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Orion.DAL.Repository.Interfaces;
using Orion.DAL.Repository;
using Microsoft.AspNetCore.Http.Features;
using Orion.Web.API.Interfaces;

namespace Orion.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Inject AppSettings
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddControllers()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddDbContext<OrionContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Orion")));

            services.AddCors();

            services.AddSwaggerGen();

            //Jwt Authentication
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());
            var allowedService = Configuration["ApplicationSettings:AllowedServiceURL"].ToString();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = allowedService,
                    ValidAudience = allowedService,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterType<UnitOfWork>().
            As<IUnitOfWork>();
            builder.RegisterType<TokenService>().
            As<ITokenService>();
            builder.RegisterType<AccessGroupRepository>().
            As<IAccessGroupRepository>();
            builder.RegisterType<CapturedTimeRepository>().
            As<ICapturedTimeRepository>();
            builder.RegisterType<RoleRepository>().
            As<IRoleRepository>();
            builder.RegisterType<TaskRepository>().
            As<ITaskRepository>();
            builder.RegisterType<UserRepository>().
            As<IUserRepository>();
            builder.RegisterType<OrionContext>().
            As<DbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            var clientUrl = Configuration["ApplicationSettings:Client_URL"].ToString();

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseCors(builder =>
            builder.WithOrigins(clientUrl)
            .AllowAnyHeader()
            .AllowAnyMethod()

            );

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orion API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
