using Microsoft.EntityFrameworkCore;
using url_shortener.Data;
using url_shortener.Data.Entities;
using url_shortener.Data.Models.Dtos.Category;
using url_shortener.Helpers;

namespace url_shortener.Services
{
    public class CategoryService
    {
        private readonly UrlShortenerContext _context;
        public CategoryService(UrlShortenerContext context)
        {
            _context = context;
        }
        public List<CategoryDto> GetAll()
        {
            return _context.Categories.Select(x => DtoHandler.GetCategoryDto(x)).ToList();
        }
        public CategoryDto? GetById(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(x => x.Id == id);
            return category is null ? null : DtoHandler.GetCategoryDto(category);
        }
        public List<CategoryDto> GetByUser(int userId)
        {
            return _context.Categories.Where(x => x.UserId == userId)
                .Select(x => DtoHandler.GetCategoryDto(x)).ToList();
        }
        public bool UserIsOwner(int userId, int categoryId)
        {
            Category? cat = _context.Categories.FirstOrDefault(x => x.Id == categoryId);
            return cat is not null && cat.UserId == userId;
        }
        public bool UserIsOwner(int userId, string categoryName)
        {
            Category? cat = _context.Categories.FirstOrDefault(x => x.Name == categoryName);
            return cat is not null && cat.UserId == userId;
        }
        public bool NameAvailable(string categoryName) 
        { 
            return _context.Categories.Any(x => x.Name == categoryName);
        }
        public int Add(CategoryForCreationDto dto, int userId)
        {
            Category newCategory = new Category() 
            {
                Name = dto.Name,
                UserId = userId
            };
            _context.Add(newCategory);
            _context.SaveChanges();
            return newCategory.Id;
        }
        public void Update(CategoryForUpdateDto dto)
        {
            Category category = _context.Categories.First(x => x.Id == dto.Id);
            category.Name = dto.Name;
            _context.Update(category);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Category category = _context.Categories.Include(x => x.Urls).Include(x => x.User).First(x => x.Id == id);
            foreach (Url url in category.Urls)
            {
                url.Categories.Remove(category);
                _context.Urls.Update(url);
            }
            category.User.Categories.Remove(category);
            _context.Users.Update(category.User);
            _context.Remove(category);
            _context.SaveChanges();
        }
    }
}
