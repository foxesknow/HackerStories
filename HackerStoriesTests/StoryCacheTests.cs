using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HackerStories;

using NUnit.Framework;

namespace HackerStoriesTests
{
    public class StoryCacheTests : TestBase
    {        
        [Test]
        public void Initialization()
        {
            var dataLoader = new MockDataLoader();
            Assert.DoesNotThrow(() => new StoryCache(m_StoryCacheSettings, dataLoader, m_Clock));
        }

        [Test]
        public async Task StoryIsLoaded()
        {
            var dataLoader = new MockDataLoader();
            dataLoader.Add("http://foo/stories/38081633.json", Story38081633);

            var cache = new StoryCache(m_StoryCacheSettings, dataLoader, m_Clock);

            var story = await cache.GetStory(38081633);
            Assert.That(dataLoader.CallCount, Is.EqualTo(1));
            
            Assert.That(story, Is.Not.Null);
            Assert.That(story.Title, Is.EqualTo("German court prohibits LinkedIn from ignoring \"Do Not Track\" signals"));
            Assert.That(story.Uri, Is.EqualTo("https://stackdiary.com/german-court-bans-linkedin-from-ignoring-do-not-track-signals/"));
            Assert.That(story.PostedBy, Is.EqualTo("isodev"));
            Assert.That(story.Score, Is.EqualTo(1187));
            Assert.That(story.CommentCount, Is.EqualTo(56));
            Assert.That(story.Time, Is.EqualTo(DateTime.Parse("2023-10-31T08:22:42Z")));
        }

        [Test]
        public async Task StoryIsLoadedAndCached()
        {
            var dataLoader = new MockDataLoader();
            dataLoader.Add("http://foo/stories/38081633.json", Story38081633);

            var cache = new StoryCache(m_StoryCacheSettings, dataLoader, m_Clock);

            var story1 = await cache.GetStory(38081633);
            Assert.That(dataLoader.CallCount, Is.EqualTo(1));
            
            Assert.That(story1, Is.Not.Null);
            Assert.That(story1.Title, Is.EqualTo("German court prohibits LinkedIn from ignoring \"Do Not Track\" signals"));
            Assert.That(story1.Uri, Is.EqualTo("https://stackdiary.com/german-court-bans-linkedin-from-ignoring-do-not-track-signals/"));
            Assert.That(story1.PostedBy, Is.EqualTo("isodev"));
            Assert.That(story1.Score, Is.EqualTo(1187));
            Assert.That(story1.CommentCount, Is.EqualTo(56));
            Assert.That(story1.Time, Is.EqualTo(DateTime.Parse("2023-10-31T08:22:42Z")));

            var story2 = await cache.GetStory(38081633);
            Assert.That(dataLoader.CallCount, Is.EqualTo(1));
            
            Assert.That(story2, Is.Not.Null);
            Assert.That(story2.Title, Is.EqualTo("German court prohibits LinkedIn from ignoring \"Do Not Track\" signals"));
            Assert.That(story2.Uri, Is.EqualTo("https://stackdiary.com/german-court-bans-linkedin-from-ignoring-do-not-track-signals/"));
            Assert.That(story2.PostedBy, Is.EqualTo("isodev"));
            Assert.That(story2.Score, Is.EqualTo(1187));
            Assert.That(story2.CommentCount, Is.EqualTo(56));
            Assert.That(story2.Time, Is.EqualTo(DateTime.Parse("2023-10-31T08:22:42Z")));
        }

        [Test]
        public async Task StoryIsLoadedAndReloadedAfterExpiry()
        {
            var dataLoader = new MockDataLoader();
            dataLoader.Add("http://foo/stories/38081633.json", Story38081633);

            var cache = new StoryCache(m_StoryCacheSettings, dataLoader, m_Clock);

            var story1 = await cache.GetStory(38081633);
            Assert.That(dataLoader.CallCount, Is.EqualTo(1));
            
            Assert.That(story1, Is.Not.Null);
            Assert.That(story1.Title, Is.EqualTo("German court prohibits LinkedIn from ignoring \"Do Not Track\" signals"));
            Assert.That(story1.Uri, Is.EqualTo("https://stackdiary.com/german-court-bans-linkedin-from-ignoring-do-not-track-signals/"));
            Assert.That(story1.PostedBy, Is.EqualTo("isodev"));
            Assert.That(story1.Score, Is.EqualTo(1187));
            Assert.That(story1.CommentCount, Is.EqualTo(56));
            Assert.That(story1.Time, Is.EqualTo(DateTime.Parse("2023-10-31T08:22:42Z")));

            m_Clock.Advance(TimeSpan.FromSeconds(30));

            var story2 = await cache.GetStory(38081633);
            Assert.That(dataLoader.CallCount, Is.EqualTo(2));
            
            Assert.That(story2, Is.Not.Null);
            Assert.That(story2.Title, Is.EqualTo("German court prohibits LinkedIn from ignoring \"Do Not Track\" signals"));
            Assert.That(story2.Uri, Is.EqualTo("https://stackdiary.com/german-court-bans-linkedin-from-ignoring-do-not-track-signals/"));
            Assert.That(story2.PostedBy, Is.EqualTo("isodev"));
            Assert.That(story2.Score, Is.EqualTo(1187));
            Assert.That(story2.CommentCount, Is.EqualTo(56));
            Assert.That(story2.Time, Is.EqualTo(DateTime.Parse("2023-10-31T08:22:42Z")));
        }
    }
}
