using System;

namespace LibraryNET6Pages
{
	public class RentRow
	{
		private string _student;
		private DateTime? _rentDate;
		private DateTime? _deadline;
		private DateTime? _returnDate;
		private bool _idDeadlineFailed;

		public string Student { get => _student; set => _student = value; }
		public DateTime? RentDate { get => _rentDate; set => _rentDate = value; }
		public DateTime? Deadline { get => _deadline; set => _deadline = value; }
		public DateTime? ReturnDate { get => _returnDate; set => _returnDate = value; }
		public bool IsDeadlineFailed { get => _idDeadlineFailed; set => _idDeadlineFailed = value; }
	}
}
