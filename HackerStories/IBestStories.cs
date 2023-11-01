namespace HackerStories
{
    /// <summary>
    /// Returns the best stores from Hacker News
    /// </summary>
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
