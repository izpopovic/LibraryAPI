using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int NumberOfBookCopies { get; set; }

        public virtual Book Book { get; set; }
        public int BookId { get; set; }
    }
}
