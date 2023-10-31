using System.Text.Json;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace HackerStories
{
    /// <summary>
    /// Loads and caches story details
    /// </summary>
    public sealed class StoryCache : IStoryCache
    {
        private readonly IDataLoader m_DataLoader;
        private readonly IClock m_Clock;

        private readonly JsonSerializerOptions m_JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly ConcurrentDictionary<long, CacheData> m_Cache = new();

        private readonly TimeSpan m_Expiry = TimeSpan.FromSeconds(10);
        private readonly string m_EndpointMask = "";

        /// <summary>
        /// Initializes the instance
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentException"></exception>
        public StoryCache(IOptions<StoryCacheSettings> options, IDataLoader dataLoader, IClock clock)
        {
            m_DataLoader = dataLoader;
            m_Clock = clock;

            var settings = options.Value;

            if(string.IsNullOrWhiteSpace(settings.EndpointMask)) throw new ArgumentException("endpoint mask is invalid", nameof(options));

            m_EndpointMask = settings.EndpointMask;
            m_Expiry = settings.Expiry;
        }

        /// <inheritdoc/>
        public Task<StoryDetails> GetStory(long storyID)
        {
            var cacheData = m_Cache.AddOrUpdate
            (
                storyID,
                id => LoadStory(id),
                (id, existing) => UpdateStory(id, existing)
            );

            return cacheData.Task.Value;
        }

        /// <summary>
        /// Connects to the server and downloads story data
        /// </summary>
        /// <param name="storyID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private CacheData LoadStory(long storyID)
        {
            var endpoint = string.Format(m_EndpointMask, storyID);

            var lazyLoad = new Lazy<Task<StoryDetails>>(async () =>
            {
                var stream = await m_DataLoader.Get(endpoint).ConfigureAwait(false);

                var incoming = JsonSerializer.Deserialize<IncomingData>(stream, m_JsonOptions);
                if(incoming is null) throw new Exception($"could not load story {storyID}");

                var storyDetails = new StoryDetails
                {
                    Title = incoming.Title,
                    Uri = incoming.Url,
                    PostedBy = incoming.By,
                    Time = FromUnixTime(incoming.Time),
                    CommentCount = incoming.Kids?.Count ?? 0,
                    Score = incoming.Score
                };

                return storyDetails;
            });

            return new(m_Clock, lazyLoad);
        }

        /// <summary>
        /// Called when there's already cached data in the dictionary
        /// and we may want to update it
        /// </summary>
        /// <param name="storyID"></param>
        /// <param name="existing"></param>
        /// <returns></returns>
        private CacheData UpdateStory(long storyID, CacheData existing)
        {
            if(existing.Age > m_Expiry)
            {
                return LoadStory(storyID);
            }

            return existing;
        }

        /// <summary>
        /// Converts the number of seconds since the Unix epoch to a DateTime
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private static DateTime FromUnixTime(long seconds)
        {
            DateTime start = DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
            return DateTime.SpecifyKind(start, DateTimeKind.Utc);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"EndpointMask = {m_EndpointMask}, Expiry = {m_Expiry}";
        }

        /// <summary>
        /// Holds the data we cache
        /// </summary>
        sealed class CacheData
        {
            private readonly IClock m_Clock;
            private readonly DateTime m_CreatedUtc;

            /// <summary>
            /// Initializes the instance
            /// </summary>
            /// <param name="task"></param>
            public CacheData(IClock clock, Lazy<Task<StoryDetails>> task)
            {
                m_Clock = clock;

                m_CreatedUtc = clock.UtcNow;
                this.Task = task;
            }

            /// <summary>
            /// Returns how old the cache dat is
            /// </summary>
            public TimeSpan Age
            {
                get{return m_Clock.UtcNow - m_CreatedUtc;}
            }

            /// <summary>
            /// The cached data
            /// </summary>
            public Lazy<Task<StoryDetails>> Task{get;}
        }

        /// <summary>
        /// Used to extract the data coming back in the request.
        /// We're using different field names in our response
        /// </summary>
        sealed class IncomingData
        {
            public string? Title{get; set;}
            public string? Url{get; set;}
            public string? By{get; set;}
            public long Time{get; set;}
            public List<long>? Kids{get; set;}
            public long Score{get; set;}

        }
    }
}
