namespace LibraryNET6Pages
{
	public class Book
	{
		private int _id;
		private string _title;
		private string _author;
		private string _genre;
		private string _description;
		private string _image;
		private int _date;

		private byte[] _byteImage;

		public int Id { get => _id; private set { _id = value; } }
		public string Title { get => _title; private set { _title = value; } }
		public string Author { get => _author; private set { _author = value; } }
		public string Genre { get => _genre; private set { _genre = value; } }
		public string Description { get => _description; private set { _description = value; } }
		public string Image { get => _image; private set { _image = value; } }
		public int Date { get => _date; private set { _date = value; } }
		public byte[] ByteImage { get => _byteImage; private set { _byteImage = value; } }

		public Book(int id, string title, string author, string genre,
			string description, string image, int date)
		{
			Id = id;
			Title = title;
			Author = author;
			Genre = genre;
			Description = description;
			Image = image;
			Date = date;
		}

		public Book(int id, string title, string author, string genre,
			string description, byte[] image, int date)
		{
			Id = id;
			Title = title;
			Author = author;
			Genre = genre;
			Description = description;
			ByteImage = image;
			Date = date;
		}

		public void ChangeDescriptionForSql()
		{
			Description = Description.Replace("\'", "\'\'");
			/*Description = Description.Replace("\"", "\"\"");*/
		}
	}
}
