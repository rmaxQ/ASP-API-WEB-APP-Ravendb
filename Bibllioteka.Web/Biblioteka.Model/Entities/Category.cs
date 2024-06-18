using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Model.Entities
{
    public class Category
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
    }
}