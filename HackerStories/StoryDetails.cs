namespace HackerStories
{
    /// <summary>
    /// Holds the details of a story
    /// </summary>
    public sealed class StoryDetails
    {
        /// <summary>
        /// The story title
        /// </summary>
        public string? Title{get; init;}

        /// <summary>
        /// The location of the story
        /// </summary>
        public string? Uri{get; init;}

        /// <summary>
        /// Who posted it
        /// </summary>
        public string? PostedBy{get; init;}

        /// <summary>
        /// When it was posted
        /// </summary>
        public DateTime Time{get; init;}

        // It's score
        public long Score{get; init;}

        /// <summary>
        /// The number of comments on the story
        /// </summary>
        public long CommentCount{get; init;}

    }
}
