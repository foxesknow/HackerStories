using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HackerStories;

using NUnit.Framework;

namespace HackerStoriesTests
{
    [TestFixture]
    public class BestStoriesTests : TestBase
    {
        private MockDataLoader m_DataLoader;

        private RankingCache m_RankingCache;
        private StoryCache m_StoryCache;

        public override void Setup()
        {
            base.Setup();

            m_DataLoader = new MockDataLoader();
            m_DataLoader.Add("http://foo/beststories.json", "[38081633]");
            m_DataLoader.Add("http://foo/stories/38081633.json", Story38081633);

            m_RankingCache = new RankingCache(MakeRankingCacheSettings(), m_DataLoader, m_Clock);
            m_StoryCache = new StoryCache(MakeStoryCacheSettings(), m_DataLoader, m_Clock);
        }

        [Test]
        public void Initialization()
        {
            Assert.DoesNotThrow(() => new BestStories(m_RankingCache, m_StoryCache));
        }

        [Test]
        public void DataNotAvailable()
        {
            m_DataLoader.Add("http://foo/beststories.json", "[38081633, 38071508]");

            var bestStories = new BestStories(m_RankingCache, m_StoryCache);

            Assert.CatchAsync(async () => await bestStories.GetBestStories(2));
        }

        [Test]
        public async Task GetBestStories()
        {
            var bestStories = new BestStories(m_RankingCache, m_StoryCache);
            var stories = await bestStories.GetBestStories(1);
            Assert.That(stories, Is.Not.Null);
            Assert.That(stories.Count, Is.EqualTo(1));

            var story = stories[0];
            Assert.That(story.Title, Is.EqualTo("German court prohibits LinkedIn from ignoring \"Do Not Track\" signals"));
            Assert.That(story.Uri, Is.EqualTo("https://stackdiary.com/german-court-bans-linkedin-from-ignoring-do-not-track-signals/"));
            Assert.That(story.PostedBy, Is.EqualTo("isodev"));
            Assert.That(story.Score, Is.EqualTo(1187));
            Assert.That(story.CommentCount, Is.EqualTo(56));
            Assert.That(story.Time, Is.EqualTo(DateTime.Parse("2023-10-31T08:22:42Z")));
        }

        [Test]
        public void GetBestStories_InvalidCount()
        {
            var bestStories = new BestStories(m_RankingCache, m_StoryCache);
            
            Assert.CatchAsync(async () => await bestStories.GetBestStories(-10));
        }

        [Test]
        public async Task StoriesAreOrdered()
        {
            m_DataLoader.Add("http://foo/beststories.json", "[38081633, 38071508]");
            m_DataLoader.Add("http://foo/stories/38071508.json", Story38071508);

            var bestStories = new BestStories(m_RankingCache, m_StoryCache);
            var stories = await bestStories.GetBestStories(2);
            Assert.That(stories, Is.Not.Null);
            Assert.That(stories.Count, Is.EqualTo(2));

            var firstStory = stories[0];
            var secondStory = stories[1];
            Assert.That(firstStory.Score, Is.GreaterThan(secondStory.Score));
        }
    }
}
