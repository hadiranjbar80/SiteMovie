using SiteMovie.Domain.Models;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SiteMovie.Service.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IRepository<Category> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IRepository<Category> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = await _repository.GetByIdAsync(categoryId);
            await _repository.DeleteAsync(category);
            await _unitOfWork.Commit();
        }

        //public IEnumerable<Category> GetAllCategories()
        //{
        //    // return _repository.GetAllAsync();
        //}

        //public async Category GetCategory(int categoryId)
        //{
        //    return await _repository.GetByIdAsync(categoryId);
        //}

        public async Task InsertCategory(Category category)
        {
            await _repository.AddAsync(category);
        }

        public async Task UpdateCategory(Category category)
        {
            await _repository.UpdateAsync(category);
        }
    }
}
