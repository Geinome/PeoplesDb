using PeoplesDb.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeoplesDb.Client.Services
{
    public interface IPeopleClient
    {
        Task<IEnumerable<Person>> GetPeople();

        Task<Person> GetPerson(int id);

        Task<bool> AddPerson(Person person);

        Task<bool> UpdatePerson(Person person);

        Task<bool> DeletePerson(int id);
    }
}
