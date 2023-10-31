namespace HackerStories
{
    /// <summary>
    /// Defines the settings for the story cache that can be loaded from application configuration
    /// </summary>
    public sealed class StoryCacheSettings
    {
        public const string Name = "StoryCache";

        /// <summary>
        /// The C# format mask used to create the endpoint to load story details from
        /// </summary>
        public string EndpointMask{get; set;} = "";

        /// <summary>
        /// How long to cache stories for before reloading from the server
        /// </summary>
        public TimeSpan Expiry{get; set;}  = TimeSpan.Zero;
    }
}
