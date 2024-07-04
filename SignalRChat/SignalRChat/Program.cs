using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Hubs;
using System.Text;

namespace SignalRChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(jwtops => 
            {
                jwtops.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("copyrightasp.netcoresignalrchatroom")),
                     
                    ValidateIssuer = true,
                    ValidIssuer="signalr chat",

                    ValidateAudience = true,
                    ValidAudience= "signalr chat",

                    ValidateLifetime = false,
                    ClockSkew=TimeSpan.Zero,
                };
                jwtops.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var header = context.Request.Headers["Authorization"];
                        //var blacklist = context.HttpContext.RequestServices.GetService<TokenBlacklistService>();
                        //if (blacklist.IsBlacklisted(header.ToString()))
                        //{
                        //    context.Response.StatusCode = 401;
                        //    context.Fail(new Exception("Authorization token blacklisted"));
                        //}

                        return Task.CompletedTask;
                    },
                    //OnTokenValidated = talid =>
                    //{
                    //    return Task.CompletedTask;
                    //}
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Privacy}/{id?}");

            app.MapHub<ChatHub>("chatHub");
            app.MapHub<StronglyTypedChatHub>("StronglyTypedChatHub");
            app.Run();
        }
    }
}