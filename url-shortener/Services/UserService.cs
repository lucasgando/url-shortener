using url_shortener.Data;
using url_shortener.Data.Entities;
using url_shortener.Data.Models.Dtos.User;
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
            return _context.Users.Select(u => DtoHandler.GetUserDto(u)).ToList();
        }
        public UserDto? Get(int id)
        {
            User? user = _context.Users.SingleOrDefault(user => user.Id == id);
            return user is null ? null : DtoHandler.GetUserDto(user);
        }
        public UserDto? Get(string email)
        {
            User? user = _context.Users.SingleOrDefault(user => user.Email == email.ToLower());
            return user is null ? null : DtoHandler.GetUserDto(user);
        }
        public bool Authenticate(string email, string password)
        {
            UserDto? user = Get(email);
            if (user == null) return false;
            return user.PasswordHash == PasswordHashing.GetPasswordHash(password);
        }
        public bool Exists(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }
        public int Add(UserForCreationDto dto)
        {
            User newUser = new User()
            {
                Username = dto.Username,
                Email = dto.Email.ToLower(),
                PasswordHash = PasswordHashing.GetPasswordHash(dto.Password)
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser.Id;
        }
        public void Update(UserForUpdateDto dto)
        {
            User user = _context.Users.Single(u => u.Email == dto.Email.ToLower());
            user.Username = dto.Username;
            user.PasswordHash = PasswordHashing.GetPasswordHash(dto.Password);
            _context.SaveChanges();
        }
        public void Delete(UserForDeletionDto dto)
        {
            User userToDelete = _context.Users.Single(u => u.Email == dto.Email);
            foreach (Category cat in userToDelete.Categories)
            {
                _context.Categories.Remove(cat);
            }
            foreach (Url url in userToDelete.Urls)
            {
                _context.Urls.Remove(url);
            }
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
        }
    }
}
