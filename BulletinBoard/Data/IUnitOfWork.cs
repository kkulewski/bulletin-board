using System.Threading.Tasks;

namespace BulletinBoard.Data
{
    public interface IUnitOfWork
    {
        Task<int> Save();
    }
}
