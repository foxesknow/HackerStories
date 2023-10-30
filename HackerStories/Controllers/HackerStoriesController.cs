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

        private readonly IAllStories m_AllStories;
        private readonly IStoryCache m_StoryCache;

        public HackerStoriesController(ILogger<HackerStoriesController> logger, IAllStories allStories, IStoryCache storyCache)
        {
            m_Logger = logger;
            m_AllStories = allStories;
            m_StoryCache = storyCache;
        }

        [HttpGet]
        public async Task<IEnumerable<StoryDetails>> GetStories(int count)
        {
            m_Logger.LogInformation("{Controller}.{Method} - getting {Count} stories", nameof(HackerStoriesController), nameof(GetStories), count);

            var allStories = await m_AllStories.GetBestStories();

            var storyDetails = new List<StoryDetails>(count);

            // We'll request story details in batches to improve throughput
            var batch = new List<Task<StoryDetails>>(m_BatchSize);

            foreach(long storyID in allStories.Take(count))
            {
                var task = m_StoryCache.GetStory(storyID);
                batch.Add(task);

                if(batch.Count == m_BatchSize)
                {
                    await ProcessBatch(batch, storyDetails);
                }
            }

            // Process anything that's left
            await ProcessBatch(batch, storyDetails);

            /*
             * The async nature of the "best stories" and story cache mean that
             * when we download the invididual stories their scores may have changed
             * since we got the "best stories" data, and this may cause the order to
             * no longer be strictly descending.
             * 
             * A final sort here will ensure the data is sorted by score
             */
            return storyDetails.OrderByDescending(story => story.Score);
        }

        private static Task ProcessBatch(List<Task<StoryDetails>> batch, List<StoryDetails> storyDetails)
        {
            if(batch.Count == 0) return Task.CompletedTask;

            return Execute(batch, storyDetails);

            static async Task Execute(List<Task<StoryDetails>> batch, List<StoryDetails> storyDetails)
            {
                await Task.WhenAll(batch);

                foreach(var task in batch)
                {
                    storyDetails.Add(task.Result);
                }

                batch.Clear();
            }
        }
    }
}