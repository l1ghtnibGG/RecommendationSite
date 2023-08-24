namespace RecommendationSite.Models.Repo
{
    public class EFUserRepository : IRecommendationRepository<User>
    {
        private readonly RecommendationDbContext _context;

        public EFUserRepository(RecommendationDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetValues => _context.Users;

        public User GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
