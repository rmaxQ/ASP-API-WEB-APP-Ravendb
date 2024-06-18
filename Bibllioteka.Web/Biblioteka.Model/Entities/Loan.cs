using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Model.Entities
{
    public class Loan
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Member ID is required.")]
        public string MemberId { get; set; }

        [Required(ErrorMessage = "Book ID is required.")]
        public string BookId { get; set; }
        [Required(ErrorMessage = "Loaned date id required.")]
        [DataType(DataType.Date)]
        public DateTime LoanedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnedAt { get; set; }
    }
}