using HRMS.Data;
using HRMS.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public interface IUserProfileRepository
    {
        Task<IEnumerable<UserProfile>> GetAllAsync(int skip = 0, int take = 1000);
        Task<UserProfile> GetByIdAsync(string id);
        Task<T> GetByIdAsync<T>(string userId) where T : IUserProfile;
        Task<bool> UserExistsAsync(string userId);
        Task<bool> ExistsAsync<T>(string userId) where T : IUserProfile;
        Task AddAsync<T>(T entity) where T : IUserProfile;
        Task UpdateAsync<T>(T entity) where T : IUserProfile;
        Task<bool> DeleteAsync<T>(string id) where T : IUserProfile;
    }
}
