using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeoplesDb.Shared
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
