﻿namespace RecommendationSite.Models.Repo
{
    public interface IRecommendationRepository<T> where T : class
    {
        public IQueryable<T> GetValues { get; }
        
        public void Save();

        public string Delete(Guid id);

        public T Add(T item);

        public T Edit(T item);
    }
}
