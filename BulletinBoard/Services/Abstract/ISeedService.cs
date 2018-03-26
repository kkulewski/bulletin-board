using System.Threading.Tasks;

namespace BulletinBoard.Services.Abstract
{
    public interface ISeedService
    {
        /// <summary>
        /// Seed data store with defined data.
        /// </summary>
        /// <returns>seed task</returns>
        Task Seed();
    }
}
