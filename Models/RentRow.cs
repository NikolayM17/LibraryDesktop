using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryNET6Pages
{
	public class RentRow
	{
		public string Student { get; set; }
		public DateTime? RentDate { get; set; }
		public DateTime? Deadline { get; set; }
		public DateTime? ReturnDate { get; set; }
		public bool IsDeadlineFailed { get; set; }
	}
}
