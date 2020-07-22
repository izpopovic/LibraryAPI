using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models.Views
{
    public class UserView
    {
		[Required(ErrorMessage = "First name is required!")]
		[StringLength(100)]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name  is required!")]
		[StringLength(50)]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Date of birth is required!")]
		public DateTime DateOfBirth { get; set; }
	}
}
