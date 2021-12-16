using System.ComponentModel.DataAnnotations;

namespace AareonTechnicalTest.Models
{
    /// <summary>
    /// This is only a request model, I considered creating a table and relationships to Ticket & Person but I want to keep it simple for this test
    /// </summary>
    public class Note
    {
        [Required]
        public int TicketId { get; set; }
        [Required]
        public int PersonId { get; set; }
        [StringLength(1024)]
        public string NoteContent { get; set; }
    }
}
