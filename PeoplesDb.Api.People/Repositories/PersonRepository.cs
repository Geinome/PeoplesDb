using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PeoplesDb.Api.People.Models;
using PeoplesDb.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeoplesDb.Api.People.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonContext personContext;

        public PersonRepository(PersonContext personContext)
        {
            this.personContext = personContext ?? throw new ArgumentNullException(nameof(personContext));
        }

        public async Task<int> AddAsync(Person entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (await personContext.People.AnyAsync(p => p.Id == entity.Id))
            {
                return entity.Id;
            }

            await personContext.People.AddAsync(entity);
            await personContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<Person> GetAsync(int id)
        {
            return await personContext.People.FindAsync(id);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return personContext.People;
        }

        public async Task<Person> RemoveAsync(int id)
        {
            Person result = await personContext.People.FindAsync(id);
            if (result != null)
            {
                personContext.People.Remove(result);
                await personContext.SaveChangesAsync();
            }

            return result;
        }

        public async Task UpdateAsync(Person entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Person existing = await personContext.People.FirstOrDefaultAsync(p => p.Id == entity.Id);
            
            if(existing != null)
            {
                existing.FirstName = entity.FirstName;
                existing.LastName = entity.LastName;

                await personContext.SaveChangesAsync();
            }
        }
    }
}
