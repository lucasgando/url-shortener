using url_shortener.Data;
using url_shortener.Data.Entities;

namespace url_shortener.Services
{
    public class UserService
    {
        private readonly UrlShortenerContext _context;
        public UserService(UrlShortenerContext urlShortenerContext)
        {
            _context = urlShortenerContext;
        }
        public List<User> GetUsers()
        {
            return _context.Users.ToList();  
        }
        public User? GetUser(int id)
        {
            return _context.Users.SingleOrDefault(user => user.Id == id);
        }
        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }
        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }
        public bool DeleteUser(int id)
        {
            User? userToDelete = GetUser(id);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
