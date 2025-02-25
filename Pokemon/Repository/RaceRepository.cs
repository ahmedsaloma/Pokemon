using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Interface;
using Pokemon.Models;

namespace Pokemon.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Race club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Race club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            var clubs = _context.Races.ToListAsync();
            return await clubs;
        }

        public async Task<Race?> GetByIdAsync(int id)
        {
            return await _context.Races.Include(i => i.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Race?> GetByIdAsyncNoTtracking(int id)
        {
            return await _context.Races.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool Update(Race club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
