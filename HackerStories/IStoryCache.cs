namespace HackerStories
{
    /// <summary>
    /// Defines a story cache
    /// </summary>
    public interface IStoryCache
    {
        /// <summary>
        /// Gets the story with the specified ID
        /// </summary>
        /// <param name="storyID"></param>
        /// <returns></returns>
        public Task<StoryDetails> GetStory(long storyID);
    }
}
