using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class RentEvent
    {
        public int Id { get; set; }
        // When he borrowed a book
        public DateTime BorrowedDate { get; set; }
        // When he returned a book
        public DateTime? ReturnDate { get; set; }
        // When should he return a book (defined by librarian)
        public DateTime DueDate { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Book Book { get; set; }
        public int BookId { get; set; }
    }
}
