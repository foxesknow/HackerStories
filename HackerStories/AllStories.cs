﻿using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HackerStories
{
    /// <summary>
    /// Loads all stories from a http endpoint
    /// </summary>
    public sealed class AllStories : IAllStories, IDisposable
    {
        private readonly HttpClient m_HttpClient = new();

        private readonly object m_SyncRoot = new ();
        
        private Lazy<Task<IReadOnlyList<long>>>? m_LazyLoad;
        private DateTime m_LastLoadUtc = DateTime.UtcNow;

        private readonly string m_Endpoint = "";
        private readonly TimeSpan m_Expiry = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Initializes the instance
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentException"></exception>
        public AllStories(IOptions<AllStoriesSettings> options)
        {
            var settings = options.Value;
            
            m_Endpoint = settings.Endpoint;
            m_Expiry = settings.Expiry;

            if(string.IsNullOrWhiteSpace(m_Endpoint)) throw new ArgumentException("endpoint is invalid", nameof(options));
        }

        
        /// <inheritdoc/>
        public void Dispose()
        {
            m_HttpClient.Dispose();
        }

        /// <inheritdoc/>
        public Task<IReadOnlyList<long>> GetBestStories()
        {
            var stories = GetOrRefreshStories();
            return stories.Value;
        }

        /// <summary>
        /// Gets the best stories.
        /// If the data has expired it will refresh the story list
        /// </summary>
        /// <returns></returns>
        private Lazy<Task<IReadOnlyList<long>>> GetOrRefreshStories()
        {
            var now = DateTime.UtcNow;

            lock(m_SyncRoot)
            {
                if(m_LazyLoad is null || (now - m_LastLoadUtc) > m_Expiry)
                {
                    m_LazyLoad = LoadBestStories();
                    m_LastLoadUtc = now;
                }

                return m_LazyLoad;
            }
        }

        /// <summary>
        /// Downloads the best stories from the server
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Lazy<Task<IReadOnlyList<long>>> LoadBestStories()
        {
            return new(async () =>
            {
                var response = await m_HttpClient.GetAsync(m_Endpoint).ConfigureAwait(false);

                var stories = JsonSerializer.Deserialize<List<long>>(response.Content.ReadAsStream());
                if(stories == null) throw new Exception("no stories found");

                return stories;
            });
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Endpoint = {m_Endpoint}, Expiry = {m_Expiry}";
        }
    }
}