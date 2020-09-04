using System.Collections.Generic;
using Bogus;

using Person = PeoplesDb.Models.Person;

namespace PeoplesDb.Api.People.Tests.Data
{
    public static class PersonGenerator
    {
        private readonly static Faker<Person> personFaker =
            new Faker<Person>()
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName());

        public static Person Generate() 
            => personFaker.Generate();

        public static IEnumerable<Person> Generate(int count) 
            => personFaker.Generate(count);
    }
}
