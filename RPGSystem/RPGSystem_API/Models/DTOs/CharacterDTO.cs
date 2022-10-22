using System.ComponentModel.DataAnnotations;

namespace RPGSystem_API.Models.DTOs
{
    public class CharacterDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
