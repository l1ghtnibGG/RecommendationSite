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

        public Review Add(Review review)
        {
            _context.Add(review);
            _context.SaveChanges();

            return review;
        }

        public Review Edit(Review review)
        {
            EditTags(review);
            
            _context.Reviews.Update(review);
            _context.SaveChanges();

            return review;
        }

        private void EditTags(Review review)
        {
            try
            {
                _context.Reviews.FirstOrDefault(x => x.Id == review.Id).Tags.
                        FirstOrDefault(x => x.Name == Review.GroupType.Book.ToString() + "s" ||
                                            x.Name == Review.GroupType.Game.ToString() + "s" ||
                                            x.Name == Review.GroupType.Movie.ToString() + "s").Name =
                    review.Tags.FirstOrDefault(x => x.Name == review.Group.ToString() + "s").Name;

                review.Tags.Remove(review.Tags.FirstOrDefault(x => 
                    x.Name == review.Group.ToString() + "s"));
            }
            catch (NullReferenceException ex)
            {
                
            }
        }

        public void Save() => _context.SaveChanges();
        
        public string Delete(Guid id)
        {
            var review = _context.Reviews.FirstOrDefault(x => x.Id == id);
            
            if (review == null)
                return "Wrong";
            
            DeleteImage(review.ImageUrl);

            _context.Remove(review);
            _context.SaveChanges();

            return $"Delete {review.Title} successfully";
        }
        
        private void DeleteImage(string imgPath)
        {
            var path = imgPath.Insert(0, "wwwroot").Replace('/', '\\');

            var f = new FileInfo(path);
            f.Delete();
        }
    }
}
