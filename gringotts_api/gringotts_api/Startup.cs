using gringotts_application.Services.Imp;
using gringotts_application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using gringotts_application;
using Microsoft.Extensions.Hosting;
using gringotts_application.Helpers;
using AutoMapper;




namespace gringotts_api
{
    /// <summary>
    /// Configuration class for setting up and configuring the Gringotts API.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration property to access application settings.
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// Constructor Startup class.
        /// </summary>
        /// <param name="configuration">Configuration object for accessing app.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures the services required by the application.
        /// </summary>
        /// <param name="services">Collection of services to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            string connectionString;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")== "Development")    
            {

                connectionString = Configuration.GetValue<string>("ConnectionStrings:gringottsDBDevelopment");
            }
            else
            {
                connectionString = Configuration.GetConnectionString("gringottsDBProd");
            }

            services.AddDbContext<GringottsDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gringotts Api", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                     { securityScheme, new string[] { } }
                };

                c.AddSecurityRequirement(securityRequirement);
                c.AddSecurityRequirement(securityRequirement);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OverseerPolicy", policy => policy.RequireClaim("role", "Overseer"));
                options.AddPolicy("MinionPolicy", policy => policy.RequireClaim("role", "Minion"));
            });


            var issuer = Configuration["AuthenticationSettings:Issuer"];
            var audience = Configuration["AuthenticationSettings:Audience"];
            var signinKey = Configuration["AuthenticationSettings:SigningKey"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signinKey))
                };
            });


            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IHouseService), typeof(HouseService));
            services.AddScoped(typeof(IMageService), typeof(MageService));
            services.AddScoped(typeof(ServiceHelper));


            services.AddCors(options =>
            {
                options.AddPolicy(name: "_miOrigenPermitidoEspecifico", policy =>
                {
                    policy.WithOrigins("http://localhost:8080", "http://localhost:4200", "https://gringottsapp.netlify.app", "https://gringottsdev.netlify.app").
                    WithMethods("GET", "POST", "PUT", "DELETE").
                    AllowAnyHeader();
                });
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

        }

        /// <summary>
        /// Configures the application's request pipeline.
        /// </summary>
        /// <param name="app">The application's request processing pipeline.</param>
        /// <param name="env">The hosting enviroment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("_miOrigenPermitidoEspecifico");

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gringotts Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

    }
}
