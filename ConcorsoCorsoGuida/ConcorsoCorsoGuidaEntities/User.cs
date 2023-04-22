using System;
using System.ComponentModel.DataAnnotations;

namespace ConcorsoCorsoGuidaEntities
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public bool Enabled { get; set; }
    }
}
