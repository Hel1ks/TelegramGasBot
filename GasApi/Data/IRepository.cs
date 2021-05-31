using GasApi.Data.Entities;
using System.Threading.Tasks;

namespace GasApi.Data
{
    public interface IRepository
    {
        Task<UserDataEntity> GetByPersonalAccountNumber(string personalAccountNumber);

        Task Create(UserDataEntity account);

        Task Update(UserDataEntity account);
    }
}
