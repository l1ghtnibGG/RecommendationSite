namespace RecommendationSite.Models.Repo;

public class EFScoreRepository : IRecommendationRepository<Score>
{
    private readonly RecommendationDbContext _context;
    
    public EFScoreRepository(RecommendationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Score> GetValues => _context.Scores;
    
    public Score GetItem(Guid id)
    {
        throw new NotImplementedException();
    }

    public string Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Score Add(Score score)
    {
        throw new NotImplementedException();
    }
}