using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryNET6Pages
{
	public class Genre
	{
		private int _id;
		private string _name;

		public int Id { get => _id; private set => _id = value; }
		public string Name { get => _name; private set => _name = value; }

		public Genre(int id, string name)
		{
			_id = id;
			_name = name;
		}
	}
}
