namespace HackerStories
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<RankingCacheSettings>(builder.Configuration.GetSection(RankingCacheSettings.Name));
            builder.Services.Configure<StoryCacheSettings>(builder.Configuration.GetSection(StoryCacheSettings.Name));

            builder.Services.AddSingleton<IBestStories, BestStories>();
            builder.Services.AddSingleton<IDataLoader, HttpDataLoader>();
            builder.Services.AddSingleton<IClock, WallClock>();
            builder.Services.AddSingleton<IRankingCache, RankingCache>();
            builder.Services.AddSingleton<IStoryCache, StoryCache>();            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}