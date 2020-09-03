using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeoplesDb.Api.People.Models;
using PeoplesDb.Api.People.Repositories;
using PeoplesDb.Api.People.Tests.Utils;
using PeoplesDb.Shared;
using Xunit;

namespace PeoplesDb.Api.People.Tests
{
    public sealed class PersonRepositoryTests
    {
        [Fact]
        public void Constructor_NullPersonContext_ThrowsArgumentNullException()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                PersonRepository personRepository = new PersonRepository(null);
            });
        }

        [Fact]
        public async Task AddAsync_NullPerson_ThrowsArgumentNullException()
        {
            //Arrange
            PersonContext personContext = CreatePersonContext();
            PersonRepository personRepository = new PersonRepository(personContext);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await personRepository.AddAsync(null);
            });
        }

        [Fact]
        public async Task AddAsync_NewPersonWithNoExistingPeople_ReturnsIdAndAddsToContext()
        {
            //Arrange
            Person expected = PersonGenerator.Generate();
            PersonContext personContext = CreatePersonContext();

            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            int result = await personRepository.AddAsync(expected);

            //Assert
            Assert.Equal(expected.Id, result);
            Person actual = personContext.People.Find(result);
            Assert.Equal(expected, actual, PersonEqualityComparer.Default);
        }

        [Fact]
        public async Task AddAsync_ExistingPersonWithIdAddingDuplicate_DuplicateNotAddedAndReturnsExistingId()
        {
            //Arrange
            const int Id = 1;
            Person expected = PersonGenerator.Generate();
            PersonContext personContext = await CreatePersonContextWithPeople(expected);

            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            int result = await personRepository.AddAsync(expected);

            //Assert
            Assert.Equal(Id, result);
            Person actual = Assert.Single(personContext.People);
            Assert.Equal(expected, actual, PersonEqualityComparer.Default);
        }

        [Fact]
        public async Task GetAsync_NoPeopleInContext_ReturnsNull()
        {
            //Arrange
            PersonContext personContext = CreatePersonContext();

            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            Person result = await personRepository.GetAsync(0);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_IdExistsInContext_ReturnsPerson()
        {
            //Arrange
            const int ExpectedId = 1;
            Person expected = PersonGenerator.Generate();

            PersonContext personContext = await CreatePersonContextWithPeople(expected);
            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            Person actual = await personRepository.GetAsync(ExpectedId);

            //Assert
            Assert.Equal(expected, actual, PersonEqualityComparer.Default);
        }

        [Fact]
        public async Task GetAllAsync_NoPeopleInContext_ReturnsEmptyQuery()
        {
            //Arrange
            PersonContext personContext = CreatePersonContext();
            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            IEnumerable<Person> result = await personRepository.GetAllAsync();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_OnePersonInContext_ReturnsSingleItem()
        {
            //Arrange
            Person expected = PersonGenerator.Generate();

            PersonContext personContext = await CreatePersonContextWithPeople(expected);

            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            IEnumerable<Person> result = await personRepository.GetAllAsync();

            //Assert
            Person actual = Assert.Single(result);
            Assert.Equal(expected, actual, PersonEqualityComparer.Default);
        }

        [Fact]
        public async Task GetAllAsync_MultiplePeopleInContext_ReturnsMultipleItems()
        {
            //Arrange
            IEnumerable<Person> expected = PersonGenerator.Generate(10);
            PersonContext personContext = await CreatePersonContextWithPeople(expected);
            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            IEnumerable<Person> actual = await personRepository.GetAllAsync();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task RemoveAsync_PersonNotInContext_ReturnsNullAndNoEntriesRemoved()
        {
            //Arrange
            Person original = PersonGenerator.Generate();
            Person entryToRemove = PersonGenerator.Generate();
            PersonContext personContext = await CreatePersonContextWithPeople(original);
            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            Person entity = await personRepository.RemoveAsync(entryToRemove.Id);

            //Assert
            Assert.Null(entity);
            Assert.NotEmpty(personContext.People);
        }

        [Fact]
        public async Task RemoveAsync_PersonExistsInContext_RemovesFromContext()
        {
            //Arrange
            Person original = PersonGenerator.Generate();
            PersonContext personContext = await CreatePersonContextWithPeople(original);
            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            Person entity = await personRepository.RemoveAsync(original.Id);

            //Assert
            Assert.NotNull(entity);
            Assert.Empty(personContext.People);
        }

        [Fact]
        public async Task UpdateAsync_NullPerson_ThrowsArgumentNullException()
        {
            //Arrange
            PersonContext personContext = CreatePersonContext();
            PersonRepository personRepository = new PersonRepository(personContext);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await personRepository.UpdateAsync(null);
            });
        }

        [Fact]
        public async Task UpdateAsync_IdExistsInContext_OriginalUpdated()
        {
            //Arrange
            Person original = PersonGenerator.Generate();
            PersonContext personContext = await CreatePersonContextWithPeople(original);
            PersonRepository personRepository = new PersonRepository(personContext);

            //Act
            Person updated = new Person
            {
                Id = original.Id,
                FirstName = original.FirstName,
                LastName = "Perkins"
            };
            await personRepository.UpdateAsync(updated);

            //Assert

            Person actual = Assert.Single(personContext.People, p => p.Id == original.Id);
            Assert.Equal(updated, actual, PersonEqualityComparer.Default);
        }

        public PersonContext CreatePersonContext()
        {
            DbContextOptions<PersonContext> options = new DbContextOptionsBuilder<PersonContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new PersonContext(options);
        }

        public Task<PersonContext> CreatePersonContextWithPeople(IEnumerable<Person> people)
        {
            return CreatePersonContextWithPeople(people.ToArray());
        }

        public async Task<PersonContext> CreatePersonContextWithPeople(params Person[] people)
        {
            PersonContext context = CreatePersonContext();
            foreach (Person person in people)
            {
                await context.AddAsync(person);
            }

            await context.SaveChangesAsync();

            return context;
        }
    }
}
