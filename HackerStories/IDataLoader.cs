namespace HackerStories
{
    /// <summary>
    /// Defines how we load data from somewhere
    /// </summary>
    public interface IDataLoader
    {
        /// <summary>
        /// Returns a stream to data a a given endpoint
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public Task<Stream> Get(string endpoint);
    }
}
