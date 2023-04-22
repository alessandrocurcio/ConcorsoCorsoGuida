using System.ComponentModel.DataAnnotations;

namespace ConcorsoCorsoGuidaEntities
{
    public class Registration
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public bool ReadPrivacy { get; set; }

    }
}