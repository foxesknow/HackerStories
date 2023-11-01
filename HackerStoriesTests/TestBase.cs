using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HackerStories;

using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace HackerStoriesTests
{
    public abstract class TestBase
    {
        protected const string Story38081633 = @"{""by"":""isodev"",""descendants"":546,""id"":38081633,""kids"":[38082326,38081757,38088650,38081876,38081758,38085610,38083542,38081739,38083936,38081728,38082567,38082221,38083419,38082330,38086457,38082713,38082112,38082850,38083336,38082099,38082700,38086114,38083366,38081748,38092807,38082086,38082504,38085980,38082569,38085365,38085740,38082518,38083220,38081946,38082106,38082501,38088211,38082706,38082691,38082693,38082516,38082920,38092424,38084725,38092151,38084328,38083217,38081883,38083241,38083268,38081656,38081745,38092285,38083882,38081944,38081826],""score"":1187,""time"":1698740562,""title"":""German court prohibits LinkedIn from ignoring \""Do Not Track\"" signals"",""type"":""story"",""url"":""https://stackdiary.com/german-court-bans-linkedin-from-ignoring-do-not-track-signals/""}";
        protected const string Story38071508 = @"{""by"":""digital55"",""descendants"":17,""id"":38071508,""kids"":[38072092,38072193,38073042,38072678,38072727],""score"":34,""time"":1698682476,""title"":""History Says 1918 Flu Killed the Young and Healthy. These Bones Say Otherwise"",""type"":""story"",""url"":""https://www.wired.com/story/history-says-the-1918-flu-killed-the-young-and-healthy-these-bones-say-otherwise/""}";

        protected IOptions<RankingCacheSettings> m_RankingCacheSettings;
        protected IOptions<StoryCacheSettings> m_StoryCacheSettings;

        protected MockClock m_Clock;

        [SetUp]
        public virtual void Setup()
        {
            m_RankingCacheSettings = MakeRankingCacheSettings();
            m_StoryCacheSettings = MakeStoryCacheSettings();

            m_Clock = new MockClock();            
        }

        protected IOptions<RankingCacheSettings> MakeRankingCacheSettings()
        {
            var settings = new RankingCacheSettings()
            {
                Endpoint = "http://foo/beststories.json",
                Expiry = TimeSpan.FromSeconds(10)
            };

            return Options.Create(settings);
        }

        protected IOptions<StoryCacheSettings> MakeStoryCacheSettings()
        {
            var settings = new StoryCacheSettings()
            {
                EndpointMask = "http://foo/stories/{0}.json",
                Expiry = TimeSpan.FromSeconds(10)
            };

            return Options.Create(settings);
        }
    }
}
