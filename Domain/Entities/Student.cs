namespace LibraryNET6Pages
{
	public class Student
	{
		private int _id;
		private string _name;
		private int _failedDeadlines;

		public int Id { get => _id; }
		public string Name { get => _name; }
		public int FailedDeadlines { get => _failedDeadlines; }

		public Student() { }

		public Student(int id, string name, int failed_deadlines)
		{
			_id = id;
			_name = name;
			_failedDeadlines = failed_deadlines;
		}
	}
}
