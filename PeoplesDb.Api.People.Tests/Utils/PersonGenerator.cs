using System.Collections.Generic;
using Bogus;

using Person = PeoplesDb.Shared.Person;

namespace PeoplesDb.Api.People.Tests.Utils
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
