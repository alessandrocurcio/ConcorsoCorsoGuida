using System;
using System.Collections.Generic;
using System.Text;

namespace ConcorsoCorsoGuidaEntities
{
    public class RegistrationRequest
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool? ReadPrivacy { get; set; }
    }
}
