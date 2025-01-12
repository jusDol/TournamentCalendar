using TournamentCalendarAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace TournamentCalendarAPI
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin() 
                           .AllowAnyMethod() 
                           .AllowAnyHeader();
                });
            }); 
            services.AddEndpointsApiExplorer(); 
            services.AddSwaggerGen();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration["ConnectionString:DefaultConnection"]));
            services.AddControllers();
            services.AddMvc();
            services.AddHealthChecks();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(); 
                app.UseSwaggerUI();  
            }
           
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
