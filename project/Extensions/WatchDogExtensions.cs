using WatchDog;
using WatchDog.src.Enums;

namespace project.Extensions
{
    public static partial class TheExtensions
    {
        public static void AddWatchDog(this WebApplicationBuilder builder)
        {
            builder.Services.AddWatchDogServices(opt =>
            {
                opt.IsAutoClear = true;
                opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;

                //opt.IsAutoClear = false;
                //opt.SetExternalDbConnString = "Server=localhost;Database=testDb;User Id=root;Password=root;";
                //opt.DbDriverOption = WatchDogDbDriverEnum.MSSQL;
            });
        } 

        public static void UseWatchDog(this WebApplication app,IConfiguration configuration)
        {
            //https://localhost:7281/watchdog
            app.UseWatchDog(opt =>
            {
                opt.WatchPageUsername = configuration["WatchDog:UserName"]??throw new ArgumentNullException("watchdoguserisnull");
                opt.WatchPagePassword = configuration["WatchDog:PassWord"] ?? throw new ArgumentNullException("watchdogpwdisnull");
            });
        }
    }
}
