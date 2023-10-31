namespace HackerStories
{
    public interface IBestStories
    {
        /// <summary>
        /// Returns the best stories
        /// </summary>
        /// <param name="count">The maximum number of best stories to return</param>
        /// <returns></returns>
        public Task<IReadOnlyList<StoryDetails>> GetBestStories(int count);
    }
}
