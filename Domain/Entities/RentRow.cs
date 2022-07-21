using System;

namespace LibraryNET6Pages
{
	public class RentRow
	{
		private int _id;
		private string _student;
		private DateTime? _rentDate;
		private DateTime? _deadline;
		private DateTime? _returnDate;
		private bool _isDeadlineFailed;

		public int Id { get => _id; }
		public string Student { get => _student; set => _student = value; }
		public DateTime? RentDate { get => _rentDate; set => _rentDate = value; }
		public DateTime? Deadline { get => _deadline; set => _deadline = value; }
		public DateTime? ReturnDate { get => _returnDate; set => _returnDate = value; }
		public bool IsDeadlineFailed { get => _isDeadlineFailed; set => _isDeadlineFailed = value; }

		public RentRow() { }

		public RentRow(int id, string name, DateTime? rentDate, DateTime? deadline, DateTime? returnDate, bool isDeadlineFailed)
		{
			_id = id;
			_student = name;
			_rentDate = rentDate;
			_deadline = deadline;
			_returnDate = returnDate;
			_isDeadlineFailed = isDeadlineFailed;
		}
	}
}
