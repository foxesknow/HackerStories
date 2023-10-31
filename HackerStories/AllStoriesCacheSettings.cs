namespace HackerStories
{
    /// <summary>
    /// Defines the settings for all stories that can be loaded from application configuration
    /// </summary>
    public sealed class AllStoriesCacheSettings
    {
        public const string Name = "AllStoriesCache";

        /// <summary>
        /// The endpoint to load all stories from
        /// </summary>
        public string Endpoint{get; set;} = "";

        /// <summary>
        /// How long to cache the all stories data for before reloading from the server
        /// </summary>
        public TimeSpan Expiry{get; set;}  = TimeSpan.Zero;
    }
}
