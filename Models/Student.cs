using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryNET6Pages
{
	public class Student
	{
		public int Id { get; }
		public string Name { get; set; }
		public int FailedDeadlines { get; set; }

		public Student(int id, string name, int failed_deadlines)
		{
			Id = id;
			Name = name;
			FailedDeadlines = failed_deadlines;
		}
	}
}
