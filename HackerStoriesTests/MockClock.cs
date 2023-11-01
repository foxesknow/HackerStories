using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HackerStories;

namespace HackerStoriesTests
{
    public class MockClock : IClock
    {
        private DateTime m_UtcNow;

        public MockClock() : this(DateTime.UtcNow)
        {
        }

        public MockClock(DateTime utcNow)
        {
            m_UtcNow = utcNow;
        }

        public void Advance(TimeSpan duration)
        {
            m_UtcNow += duration;
        }

        public DateTime UtcNow
        {
            get{return m_UtcNow;}
        }
    }
}
