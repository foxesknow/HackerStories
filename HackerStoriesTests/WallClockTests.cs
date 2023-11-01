using HackerStories;

using NUnit.Framework;

namespace HackerStoriesTests
{
    [TestFixture]
    public class WallClockTests
    {
        [Test]
        public async Task CheckClockAdvances()
        {
            var clock = new WallClock();
            var start = clock.UtcNow;

            await Task.Delay(1_000);

            var stop = clock.UtcNow;
            Assert.That(stop, Is.GreaterThan(start));
        }
    }
}