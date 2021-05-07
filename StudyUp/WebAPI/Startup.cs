using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using BusinessLogicInterface;
using DataAccess;
using DataAccessInterface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebAPI.Filters;

namespace StudyUp
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

            services.AddCors(cors =>
            {
                cors.AddPolicy("StudyUpPolicy", options =>
                {
                    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }); ;

            services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });
            services.AddControllers();

            services.AddDbContext<DbContext, DBContext>(
                o => o.UseSqlServer(Configuration.GetConnectionString("StudyUp"), b => b.MigrationsAssembly("WebAPI"))
            );

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IDeckLogic, DeckLogic>();
            services.AddScoped<IFlashcardLogic, FlashcardLogic>();
            services.AddScoped<IGroupLogic, GroupLogic>();
            services.AddScoped<IExamLogic, ExamLogic>();
            services.AddScoped<IExamCardLogic, ExamCardLogic>();
            services.AddScoped<INotifications, FirebaseNotificationsImplementation>();

            services.AddScoped<ExceptionFilter>();

            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Study-Up {groupName}",
                    Version = groupName,
                    Description = "Study-Up api",
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyUp");
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
