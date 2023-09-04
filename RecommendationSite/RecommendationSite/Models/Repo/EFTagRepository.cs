namespace RecommendationSite.Models.Repo
{
    public class EFTagRepository : IRecommendationRepository<Tag>
    {
        private readonly RecommendationDbContext _context;

        public EFTagRepository(RecommendationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> GetValues => _context.Tags;

        public string Add(UserRegistration user)
        {
            throw new NotImplementedException();
        }

        public User Authenticate(UserLogIn user)
        {
            throw new NotImplementedException();
        }

        public Tag GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
