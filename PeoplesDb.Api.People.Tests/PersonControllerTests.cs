using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeoplesDb.Api.People.Controllers;
using PeoplesDb.Api.People.Repositories;
using PeoplesDb.Api.People.Tests.Data;
using PeoplesDb.Models;
using PeoplesDb.Utils;
using Xunit;

namespace PeoplesDb.Api.People.Tests
{
    public sealed class PersonControllerTests
    {
        [Fact]
        public void Constructor_NullRepository_ThrowsArgumentNullException()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                PersonController controller = new PersonController(null);
            });
        }

        [Fact]
        public async Task Get_NoPeopleInRepository_ReturnsEmptyCollection()
        {
            //Arrange
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            PersonController controller = new PersonController(repository.Object);

            //Act
            IEnumerable<Person> result = await controller.Get();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_SinglePersonInRepository_ReturnsSinglePerson()
        {
            //Arrange
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            repository.Setup(r => r.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Person>>(new [] { PersonGenerator.Generate() }));

            PersonController controller = new PersonController(repository.Object);

            //Act
            IEnumerable<Person> result = await controller.Get();

            //Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task Get_MultiplePeopleInRepository_ReturnsAllItems()
        {
            //Arrange
            IEnumerable<Person> people = PersonGenerator.Generate(5);
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            repository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(people));

            PersonController controller = new PersonController(repository.Object);

            //Act
            IEnumerable<Person> result = await controller.Get();

            //Assert
            Assert.Equal(people, result);
        }

        [Fact]
        public async Task GetById_PersonWithIdInRepository_ReturnsSuccessWithPerson()
        {
            //Arrange
            const int Id = 0;
            Person expected = PersonGenerator.Generate();
            expected.Id = Id;

            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            repository.Setup(r => r.GetAsync(Id))
                .Returns(Task.FromResult(expected));

            PersonController controller = new PersonController(repository.Object);

            //Act
            IActionResult result = await controller.Get(Id);

            //Assert

            
            ObjectResult resultWithValue = Assert.IsType<ObjectResult>(result);
            Person actual = Assert.IsType<Person>(resultWithValue.Value);
            Assert.Equal(expected, actual, PersonEqualityComparer.Default);
        }

        [Fact]
        public async Task GetById_NoPersonWithIdInRepository_ReturnsNotFound()
        {
            //Arrange
            const int Id = 0;
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();

            PersonController controller = new PersonController(repository.Object);

            //Act
            IActionResult result = await controller.Get(Id);

            //Assert
            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Post_NullPerson_ReturnsEmptyRequestBodyError()
        {
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();

            PersonController controller = new PersonController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "A non-empty request body is required.");

            //Act
            IActionResult result = await controller.Post(null);

            //Assert
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Post_EmptyPerson_ReturnsMultipleErrors()
        {
            //Arrange
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();

            PersonController controller = new PersonController(repository.Object);
            controller.ModelState.AddModelError(nameof(Person.FirstName), "The FirstName field is required.");
            controller.ModelState.AddModelError(nameof(Person.LastName), "The LastName field is required.");

            //Act
            IActionResult result = await controller.Post(null);

            //Assert
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Post_PersonWithFullName_ReturnsCreatedAtResult()
        {
            //Arrange
            Person expected = PersonGenerator.Generate();
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            PersonController controller = new PersonController(repository.Object);

            //Act
            IActionResult result = await controller.Post(expected);

            //Assert
            CreatedAtActionResult createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Null(createdAtActionResult.ControllerName);
            Assert.Equal(nameof(PersonController.Get), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task Put_InvalidPerson_ReturnsEmptyRequestBodyError()
        {
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();

            PersonController controller = new PersonController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "A non-empty request body is required.");

            //Act
            IActionResult result = await controller.Put(-1, null);

            //Assert
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Put_NoPeopleInRepository_ReturnsNotFound()
        {
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            PersonController controller = new PersonController(repository.Object);
            Person person = PersonGenerator.Generate();

            //Act
            IActionResult result = await controller.Put(0, person);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_ValidPersonWithExistingInRepository_ReturnsOk()
        {
            Person person = PersonGenerator.Generate();

            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            repository.Setup(r => r.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(person));

            PersonController controller = new PersonController(repository.Object);

            //Act
            IActionResult result = await controller.Put(0, person);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ValidIdNoMatchingPerson_ReturnsNotFound()
        {
            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            repository.Setup(r => r.GetAsync(It.IsAny<int>())).Returns(Task.FromResult((Person)null));

            PersonController controller = new PersonController(repository.Object);

            //Act
            IActionResult result = await controller.Delete(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ValidIdMatchingPersonInRepository_ReturnsOk()
        {
            Person person = PersonGenerator.Generate();

            Mock<IPersonRepository> repository = new Mock<IPersonRepository>();
            repository.Setup(r => r.RemoveAsync(It.IsAny<int>())).Returns(Task.FromResult(person));

            PersonController controller = new PersonController(repository.Object);

            //Act
            IActionResult result = await controller.Delete(0);

            //Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
