using url_shortener.Data;
using url_shortener.Data.Entities;
using url_shortener.Data.Models.Dtos;
using url_shortener.Helpers;

namespace url_shortener.Services
{
    public class UserService
    {
        private readonly UrlShortenerContext _context;
        public UserService(UrlShortenerContext urlShortenerContext)
        {
            _context = urlShortenerContext;
        }
        public List<User> GetAll()
        {
            return _context.Users.ToList();  
        }
        public User? GetById(int id)
        {
            return _context.Users.SingleOrDefault(user => user.Id == id);
        }
        public User? GetByEmail(string email)
        {
            return _context.Users.SingleOrDefault(user => user.Email == email.ToLower());
        }
        public bool Authenticate(string email, string password)
        {
            User? user = GetByEmail(email);
            if (user == null) return false;
            return user.PasswordHash == PasswordHashing.GetPasswordHash(password);
        }
        public bool Exists(string email)
        {
            return GetByEmail(email.ToLower()) != null;
        }
        public void Add(UserForCreationDto dto)
        {
            User newUser = new User()
            {
                Username = dto.Username.ToLower(),
                Email = dto.Email.ToLower(),
                PasswordHash = PasswordHashing.GetPasswordHash(dto.Password)
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
        public bool Update(UserForUpdateDto dto)
        {
            User? userToUpdate = GetByEmail(dto.Email);
            if (userToUpdate == null) return false;
            if (dto.Username != null) userToUpdate.Username = dto.Username;
            if (dto.Email != null) userToUpdate.Email = dto.Email;
            if (dto.Password != null) userToUpdate.PasswordHash = dto.Password;
            _context.Users.Update(userToUpdate);
            _context.SaveChanges();
            return true;
        }
        public bool Delete(UserForDeletionDto dto)
        {
            if (!Authenticate(dto.Email, dto.Password)) return false;
            User? userToDelete = GetByEmail(dto.Email);
            if (userToDelete == null) return false;
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            return true;
        }
    }
}
