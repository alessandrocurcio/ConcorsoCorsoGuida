using System.ComponentModel.DataAnnotations;

namespace ConcorsoCorsoGuidaEntities
{
    public class Extraction
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int IdRegistration { get; set; }
        public DateTime Date { get; set; }

    }
}