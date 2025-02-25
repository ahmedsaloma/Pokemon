using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Interface;
using Pokemon.Models;


namespace Pokemon.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public bool Add(AppUser user)
        {
            throw new NotImplementedException();

        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return  _applicationDbContext.Users.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _applicationDbContext.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _applicationDbContext.Update(user);
            return Save();
        }
    }
}
