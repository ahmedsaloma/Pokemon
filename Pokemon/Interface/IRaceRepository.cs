using Pokemon.Models;

namespace Pokemon.Interface
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAll();
        Task<Race?> GetByIdAsync(int id);
        Task<Race?> GetByIdAsyncNoTtracking(int id);

        Task<IEnumerable<Race>> GetRaceByCity(string city);
        bool Add(Race race);

        bool Update(Race race);

        bool Delete(Race race);

        bool Save();
    }
}
