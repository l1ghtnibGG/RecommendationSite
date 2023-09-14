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

        public User Add(User userRegistration)
        {
            var user = new User
            {
                Email = userRegistration.Email,
                Name = userRegistration.Name,
                Password = userRegistration.Password,
                Status = User.StatusType.User,
                CreatedDate = DateTime.Now,
                LastLogin = DateTime.Now,
                
            };

            _context.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public string Delete(Guid Id)
        {
            var user = _context.Users.First(x => x.Id == Id);
            _context.Remove(user);
            _context.SaveChanges();

            return $"Delete {user.Email} successfully";
        }
    }
}
