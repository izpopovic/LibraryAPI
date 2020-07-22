using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
	public class User
	{
		public int Id { get; set; }
		[Required]
		[StringLength(100)]
		public string FirstName { get; set; }
		[Required]
		[StringLength(50)]
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; } //is required by default

        public bool MZRScan { get; set; }
        public bool IsValid { get; set; }

        // one user can have many contacts
        public virtual ICollection<Contact> Contacts { get; set; }
	}
}
