namespace RecommendationSite.Models.Repo;

public class EFCommentRepository : IRecommendationRepository<Comment>
{
    private readonly RecommendationDbContext _context;
    
    public EFCommentRepository(RecommendationDbContext context)
    {
        _context = context;
    }

    IQueryable<Comment> IRecommendationRepository<Comment>.GetValues => _context.Comments;

    public void Save()
    {
        throw new NotImplementedException();
    }

    public string Delete(Guid id)
    {
        var comment = _context.Comments.FirstOrDefault(x => x.Id == id);
        
        if (comment == null)
            return "Wrong";

        _context.Remove(comment);
        _context.SaveChanges();
        
        return $"Delete {comment.Title} successfully";
    }

    public Comment Add(Comment comment)
    {
        _context.Add(comment);
        _context.SaveChanges();

        return comment;
    }

    public Comment Edit(Comment item)
    {
        throw new NotImplementedException();
    }
}