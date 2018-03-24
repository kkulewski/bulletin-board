using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Models;

namespace BulletinBoard.Data.Repositories.Abstract
{
    public interface IJobOfferRepository
    {
        Task<IList<JobOffer>> GetAll();

        Task<JobOffer> GetById(string id);

        void Add(JobOffer item);

        void Update(JobOffer item);

        void Delete(JobOffer item);
    }
}
