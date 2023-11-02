using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using HackerStories;

namespace HackerStoriesTests
{
    internal class MockDataLoader : IDataLoader
    {
        private readonly Dictionary<string, string> m_Data = new();

        public void Add(string endpoint, string json)
        {
            m_Data[endpoint] = json;
        }

        public int CallCount{get; private set;}

        public void Clear()
        {
            m_Data.Clear();
        }

        public Task<Stream> Get(string endpoint)
        {
            this.CallCount++;

            if(m_Data.TryGetValue(endpoint, out var json))
            {
                var bytes = Encoding.UTF8.GetBytes(json);
                Stream stream = new MemoryStream(bytes);

                return Task.FromResult(stream);
            }

            throw new IOException("could not load from endpoint");
        }
    }
}
