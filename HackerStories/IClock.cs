namespace HackerStories
{
    /// <summary>
    /// Defines a clock that will be used to work out expiry times
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Returns the current time in Utc
        /// </summary>
        public DateTime UtcNow{get;}
    }
}
