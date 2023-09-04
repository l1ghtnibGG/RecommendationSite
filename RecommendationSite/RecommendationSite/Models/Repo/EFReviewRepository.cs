namespace RecommendationSite.Models.Repo
{
    public class EFReviewRepository : IRecommendationRepository<Review>
    {
        private readonly RecommendationDbContext _context;
        public EFReviewRepository(RecommendationDbContext context)
        {
            _context = context;
        }

        IQueryable<Review> IRecommendationRepository<Review>.GetValues => _context.Reviews;

        public string Add(UserRegistration user)
        {
            throw new NotImplementedException();
        }

        public User? Authenticate(UserLogIn user)
        {
            throw new NotImplementedException();
        }

        public Review GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
