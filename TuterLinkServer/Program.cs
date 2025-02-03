using Microsoft.EntityFrameworkCore;
using TutorLinkServer.Hubs;
using TutorLinkServer.Models;
namespace TutorLinkServer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Add Database to dependency injection
            builder.Services.AddDbContext<NoaDBcontext>(
                    options => options.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=TuterLink_DB;User ID=TaskAdminLogin;Password=NoaF1197;; Trusted_Connection = True; MultipleActiveResultSets = true"));


            // Add Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();
            //Add Session
            app.UseSession(); //In order to enable session management

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub"); //Map the chatHub to the /chatHub URL
            app.Run();
     
        }
    }
}
