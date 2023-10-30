namespace HackerStories
{
    /// <summary>
    /// A clock implementation that returns the current time of day
    /// </summary>
    public sealed class WallClock : IClock
    {
        /// <inheritdoc/>
        public DateTime UtcNow
        {
            get{return DateTime.UtcNow;}
        }
    }
}
