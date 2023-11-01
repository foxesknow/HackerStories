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
    public class RankingCacheTests : TestBase
    {
        [Test]
        public void Initialization()
        {
            var dataLoader = new MockDataLoader();
            dataLoader.Add("http://foo/beststories.json", "[38081633,38069710,38078063]");

            Assert.DoesNotThrow(() => new RankingCache(m_RankingCacheSettings, dataLoader, m_Clock));
        }

        [Test]
        public async Task DataIsLoaded()
        {
            var dataLoader = new MockDataLoader();
            dataLoader.Add("http://foo/beststories.json", "[38081633,38069710,38078063]");

            var cache = new RankingCache(m_RankingCacheSettings, dataLoader, m_Clock);
            
            var rankings = await cache.GetBestStories();
            Assert.That(dataLoader.CallCount, Is.EqualTo(1));

            Assert.That(rankings, Is.Not.Null & Has.Count.EqualTo(3));
            Assert.That(rankings[0], Is.EqualTo(38081633));
            Assert.That(rankings[1], Is.EqualTo(38069710));
            Assert.That(rankings[2], Is.EqualTo(38078063));
        }

        [Test]
        public async Task DataIsLoadedAndCached()
        {
            var dataLoader = new MockDataLoader();
            dataLoader.Add("http://foo/beststories.json", "[38081633,38069710,38078063]");

            var cache = new RankingCache(m_RankingCacheSettings, dataLoader, m_Clock);
            
            var rankings1 = await cache.GetBestStories();
            Assert.That(dataLoader.CallCount, Is.EqualTo(1));

            Assert.That(rankings1, Is.Not.Null & Has.Count.EqualTo(3));
            Assert.That(rankings1[0], Is.EqualTo(38081633));
            Assert.That(rankings1[1], Is.EqualTo(38069710));
            Assert.That(rankings1[2], Is.EqualTo(38078063));

            var rankings2 = await cache.GetBestStories();
            Assert.That(dataLoader.CallCount, Is.EqualTo(1));

            Assert.That(rankings2, Is.Not.Null & Has.Count.EqualTo(3));
            Assert.That(rankings2[0], Is.EqualTo(38081633));
            Assert.That(rankings2[1], Is.EqualTo(38069710));
            Assert.That(rankings2[2], Is.EqualTo(38078063));
        }

        [Test]
        public async Task DataIsLoadedAndReloadedAfterExpiry()
        {
            var dataLoader = new MockDataLoader();
            dataLoader.Add("http://foo/beststories.json", "[38081633,38069710,38078063]");

            var cache = new RankingCache(m_RankingCacheSettings, dataLoader, m_Clock);
            
            var rankings1 = await cache.GetBestStories();
            Assert.That(dataLoader.CallCount, Is.EqualTo(1));

            Assert.That(rankings1, Is.Not.Null & Has.Count.EqualTo(3));
            Assert.That(rankings1[0], Is.EqualTo(38081633));
            Assert.That(rankings1[1], Is.EqualTo(38069710));
            Assert.That(rankings1[2], Is.EqualTo(38078063));

            m_Clock.Advance(TimeSpan.FromSeconds(20));

            var rankings2 = await cache.GetBestStories();
            Assert.That(dataLoader.CallCount, Is.EqualTo(2));

            Assert.That(rankings2, Is.Not.Null & Has.Count.EqualTo(3));
            Assert.That(rankings2[0], Is.EqualTo(38081633));
            Assert.That(rankings2[1], Is.EqualTo(38069710));
            Assert.That(rankings2[2], Is.EqualTo(38078063));
        }

        
    }
}
