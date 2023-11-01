using System.Net;

namespace HackerStories
{
    /// <summary>
    /// A data loader that loads from a http endpoint
    /// </summary>
    public sealed class HttpDataLoader : IDataLoader, IDisposable
    {
        private readonly HttpClient m_HttpClient = new();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_HttpClient.Dispose();
        }

        /// <inheritdoc/>
        public async Task<Stream> Get(string endpoint)
        {
            var response = await m_HttpClient.GetAsync(endpoint).ConfigureAwait(false);

            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                
            }

            throw new HttpRequestException($"error {response.StatusCode} reading from {endpoint}", null, response.StatusCode);
        }
    }
}
