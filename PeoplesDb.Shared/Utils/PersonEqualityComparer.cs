using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PeoplesDb.Models;

namespace PeoplesDb.Utils
{
    public class PersonEqualityComparer : IEqualityComparer<Person>
    {
        public static PersonEqualityComparer Default { get; }
            = new PersonEqualityComparer();

        public bool Equals([AllowNull] Person x, [AllowNull] Person y)
        {
            return (x?.FirstName?.Equals(y?.FirstName) ?? false)
                && (x?.LastName?.Equals(y?.LastName) ?? false)
                && x?.Id == y?.Id;
        }

        public int GetHashCode([DisallowNull] Person obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
