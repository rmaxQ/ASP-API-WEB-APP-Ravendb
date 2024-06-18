using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Model.Entities
{
    public class Book
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }
        [MinLength(1, ErrorMessage = "At least one author is required.")]
        public List<string> Authors { get; set; }

        [Range(-600, 2024, ErrorMessage = "Year published must be a number between -600 and 2024.")]
        public int YearPublished { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public string CategoryId { get; set; }

        [StringLength(2000, ErrorMessage = "Summary cannot be longer than 2000 characters.")]
        public string Summary { get; set; }
    }
}