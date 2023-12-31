﻿namespace HackerStories
{
    public interface IRankingCache
    {
        /// <summary>
        /// Returns the best stories, ordered from highest score to lowest score
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyList<long>> GetBestStories();
    }
}
