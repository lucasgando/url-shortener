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
        public IEnumerable<UserDto> GetAll()
        {
            return _context.Users.Select(u => new UserDto()
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Role = u.Role,
            }).ToList();
        }
        public UserDto? GetById(int id)
        {
            User? user = _context.Users.SingleOrDefault(user => user.Id == id);
            return user is null ? null : new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                Role = user.Role
            };
        }
        public UserDto? GetByEmail(string email)
        {
            User? user = _context.Users.SingleOrDefault(user => user.Email == email.ToLower());
            return user is null ? null : new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                Role = user.Role
            };
        }
        public bool Authenticate(string email, string password)
        {
            UserDto? user = GetByEmail(email);
            if (user == null) return false;
            return user.PasswordHash == PasswordHashing.GetPasswordHash(password);
        }
        public bool Exists(string email)
        {
            return GetByEmail(email.ToLower()) is not null;
        }
        public void Add(UserForCreationDto dto)
        {
            User newUser = new User()
            {
                Username = dto.Username,
                Email = dto.Email.ToLower(),
                PasswordHash = PasswordHashing.GetPasswordHash(dto.Password)
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
        public bool Update(UserForUpdateDto dto)
        {
            UserDto? oldUser = GetByEmail(dto.Email);
            if (oldUser is null) return false;
            User updatedUser = new User()
            {
                Id = oldUser.Id,
                Username = oldUser.Username,
                PasswordHash = oldUser.PasswordHash,
                Email = oldUser.Email,
                Role = oldUser.Role
            };
            if (dto.Username is not null) updatedUser.Username = dto.Username;
            if (dto.Email is not null) updatedUser.Email = dto.Email;
            if (dto.Password is not null) updatedUser.PasswordHash = dto.Password;
            _context.Users.Update(updatedUser);
            _context.SaveChanges();
            return true;
        }
        public bool Delete(UserForDeletionDto dto)
        {
            UserDto userToDelete = GetByEmail(dto.Email)!;
            User user = new User()
            {
                Id = userToDelete.Id,
                Username = userToDelete.Username,
                PasswordHash = userToDelete.PasswordHash,
                Email = userToDelete.Email,
                Role = userToDelete.Role
            };
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
