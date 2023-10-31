namespace HackerStories
{
    public interface IAllStoriesCache
    {
        /// <summary>
        /// Returns the best stories, ordered from highest score to lowest score
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyList<long>> GetBestStories();
    }
}
