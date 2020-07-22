using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
	public class Book
	{
		public int Id { get; set; }
		[Required]
		[StringLength(1000)]
		public string Title { get; set; }
		[Required]
		[StringLength(150)]
		public string Author { get; set; }
	}
}
