namespace HackerStories
{
    /// <summary>
    /// Generates the best stories list from caches
    /// </summary>
    public sealed class BestStories : IBestStories
    {
        // We'll derive a batch size based on the processor count, but capped
        // to avoid having to many simultation calls to GetStory outstanding.
        private readonly int m_BatchSize = Math.Max(16, Environment.ProcessorCount);
        
        private readonly IAllStoriesCache m_AllStories;
        private readonly IStoryCache m_StoryCache;

        /// <summary>
        /// Initializes the instance
        /// </summary>
        /// <param name="allStories">The all stories cache</param>
        /// <param name="storyCache">The individual story cache</param>
        public BestStories(IAllStoriesCache allStories, IStoryCache storyCache)
        {
            m_AllStories = allStories;
            m_StoryCache = storyCache;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<StoryDetails>> GetBestStories(int count)
        {
            if(count < 0) throw new ArgumentException($"invalid count: {count}", nameof(count));

            var allStories = await m_AllStories.GetBestStories();

            var storyDetails = new List<StoryDetails>(count);

            // We'll request story details in batches to improve throughput
            var batch = new List<Task<StoryDetails>>(m_BatchSize);

            foreach (long storyID in allStories.Take(count))
            {
                var task = m_StoryCache.GetStory(storyID);
                batch.Add(task);

                if (batch.Count == m_BatchSize)
                {
                    await WaitForBatch(batch, storyDetails);
                }
            }

            // Process anything that's left
            await WaitForBatch(batch, storyDetails);

            /*
             * The async nature of the "best stories" and story cache mean that
             * when we download the invididual stories their scores may have changed
             * since we got the "best stories" data, and this may cause the order to
             * no longer be strictly descending.
             * 
             * A final sort here will ensure the data is sorted by score
             */
            return storyDetails.OrderByDescending(story => story.Score).ToList();
        }

        /// <summary>
        /// Waits for a batch of tasks to complete and add the resulting story details to a collection
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="storyDetails"></param>
        /// <returns></returns>
        private static Task WaitForBatch(List<Task<StoryDetails>> batch, List<StoryDetails> storyDetails)
        {
            if (batch.Count == 0) return Task.CompletedTask;

            return Execute(batch, storyDetails);

            static async Task Execute(List<Task<StoryDetails>> batch, List<StoryDetails> storyDetails)
            {
                await Task.WhenAll(batch);

                foreach (var task in batch)
                {
                    storyDetails.Add(task.Result);
                }

                batch.Clear();
            }
        }
    }
}
