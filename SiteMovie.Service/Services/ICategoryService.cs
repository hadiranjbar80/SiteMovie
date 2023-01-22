using SiteMovie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SiteMovie.Service.Services
{
    public interface ICategoryService
    {
       // IEnumerable<Category> GetAllCategories();
     //   Category GetCategory(int categoryId);
        Task InsertCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int categoryId);
    }
}
