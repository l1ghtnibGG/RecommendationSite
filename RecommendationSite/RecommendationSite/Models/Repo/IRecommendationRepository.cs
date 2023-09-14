namespace RecommendationSite.Models.Repo
{
    public interface IRecommendationRepository<T> where T : class
    {
        public IQueryable<T> GetValues { get; }
        
        public T GetItem(Guid id);

        public string Delete(Guid id);

        public T Add(T item);
    }
}
