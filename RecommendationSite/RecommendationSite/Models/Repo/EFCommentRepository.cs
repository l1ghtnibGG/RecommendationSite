namespace RecommendationSite.Models.Repo;

public class EFCommentRepository : IRecommendationRepository<Comment>
{
    private readonly RecommendationDbContext _context;
    
    public EFCommentRepository(RecommendationDbContext context)
    {
        _context = context;
    }

    IQueryable<Comment> IRecommendationRepository<Comment>.GetValues => _context.Comments;

    public Comment GetItem(Guid id)
    {
        throw new NotImplementedException();
    }

    public string Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Comment Add(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Comment Edit(Comment item)
    {
        throw new NotImplementedException();
    }
}