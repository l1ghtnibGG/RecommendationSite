using Microsoft.EntityFrameworkCore;

namespace RecommendationSite.Models
{
    public class RecommendationDbContext : DbContext
    {
        public RecommendationDbContext(DbContextOptions<RecommendationDbContext> options) :
            base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
