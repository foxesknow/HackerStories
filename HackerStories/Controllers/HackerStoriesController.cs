using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace HackerStories.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class HackerStoriesController : ControllerBase
    {
        private readonly int m_BatchSize = Environment.ProcessorCount;

        private readonly ILogger<HackerStoriesController> m_Logger;

        private readonly IBestStories m_BestStories;

        public HackerStoriesController(ILogger<HackerStoriesController> logger, IBestStories bestStories)
        {
            m_Logger = logger;
            m_BestStories = bestStories;
        }

        [HttpGet]
        public Task<IReadOnlyList<StoryDetails>> GetStories(int count)
        {
            m_Logger.LogInformation("{Controller}.{Method} - getting {Count} stories", nameof(HackerStoriesController), nameof(GetStories), count);

            return m_BestStories.GetBestStories(count);
        }
    }
}