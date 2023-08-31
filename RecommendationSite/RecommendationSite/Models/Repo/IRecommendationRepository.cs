namespace RecommendationSite.Models.Repo
{
    public interface IRecommendationRepository<T> where T : class
    {
        public IQueryable<T> GetValues { get; }
        
        public T GetItem(Guid id);

        public void Save();

        public User? Authenticate(UserLogIn user);
    }
}
