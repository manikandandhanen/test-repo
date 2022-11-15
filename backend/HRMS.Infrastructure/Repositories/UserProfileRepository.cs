using HRMS.Data;
using HRMS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync(int skip = 0, int take = 1000)
        {
            return await _context.UserProfiles
                .Include(s => s.UserContactInfo)
                .Include(s => s.UserBasicInfo)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<UserProfile> GetByIdAsync(string id)
        {
            return await _context.UserProfiles
                .Include(s => s.UserBasicInfo.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            return await _context.Users.AnyAsync(s => s.Id == userId);
        }

        public async Task<bool> ExistsAsync<T>(string userId) where T: IUserProfile
        {
            return await _context.Set<T>().AnyAsync(s=>s.UserId == userId);
        }

        public async Task<T> GetByIdAsync<T>(string id) where T : IUserProfile
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync<T>(T entity) where T: IUserProfile
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T: IUserProfile
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync<T>(string id) where T: IUserProfile
        {
            T existing = await GetByIdAsync<T>(id);
            _context.Set<T>().Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
