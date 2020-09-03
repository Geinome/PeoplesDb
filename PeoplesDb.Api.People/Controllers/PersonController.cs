using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeoplesDb.Api.People.Repositories;
using PeoplesDb.Shared;

namespace PeoplesDb.Api.People.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            this.personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<Person>> Get()
        {
            return await personRepository.GetAllAsync();
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int id)
        {
            var person = await personRepository.GetAsync(id);
            if (person == null)
            {
                return StatusCode(404);
            }

            return StatusCode(200, person);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int id = await personRepository.AddAsync(person);

            return CreatedAtAction(nameof(Get), id);
        }

        [HttpPut("{id?}")]
        public async Task<IActionResult> Put(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await personRepository.GetAsync(id) == null)
            {
                return NotFound();
            }

            await personRepository.UpdateAsync(person);

            return Ok();
        }

        [HttpDelete("{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await personRepository.RemoveAsync(id) != null)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
