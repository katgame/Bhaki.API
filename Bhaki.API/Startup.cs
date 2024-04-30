using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Dice.API.Data;
using Dice.API.Data.Models;
using Dice.API.Data.Services;
using Dice.API.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dice.API.Interfaces;

namespace Dice.API
{
    public class Startup
    {
        public string ConnectionString { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
        }
        string policyName = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            //Configure DBContext with SQL
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

            //Configure the Services

            services.AddTransient<LogsService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddApiVersioning(config => 
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;

                //config.ApiVersionReader = new HeaderApiVersionReader("custom-version-header");
                //config.ApiVersionReader = new MediaTypeApiVersionReader();
            });


            //Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //Add Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //Add JWT Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT:Secret"])),

                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Audience"]
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dice.API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
            });
          
            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName,
                                  builder =>
                                  {
                                      builder
                                        //.WithOrigins("http://localhost:4200") // specifying the allowed origin
                                        //.WithOrigins("http://160.119.253.205:81/") // specifying the allowed origin
                                        //.WithOrigins("https://160.119.253.205:444/") // specifying the allowed origin
                                        //.WithMethods("GET","POST","PUT","DELETE","") // defining the allowed HTTP method
                                        .AllowAnyMethod().AllowAnyOrigin()
                                        .AllowAnyHeader(); // allowing any header to be sent
                                  });
            });
        }
       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dice.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            //CORS
            app.UseCors(policyName);
            //Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            //Exception Handling
            app.ConfigureBuildInExceptionHandler(loggerFactory);
            //app.ConfigureCustomExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //AppDbInitializer.Seed(app);
            AppDbInitializer.SeedRoles(app).Wait();
        }
    }
}
