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
