using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace LibraryAPI.Models
{
	// Pitati, to su vjerovatno drugi ljudi?
	public class Contact
	{
		public int Id { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
    }
}
