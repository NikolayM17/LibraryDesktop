namespace LibraryNET6Pages
{
	public class Book
	{
		private int _id;
		private string _title;
		private string? _author;
		private string? _genre;
		private string? _description;
		private string? _image;
		private int _date = -1;
		private int _maxCount = -1;
		private long _barcode = -1;

		private byte[] _byteImage;

		public int Id { get => _id; private set { _id = value; } }
		public string Title { get => _title; private set { _title = value; } }
		public string? Author { get => _author; private set { _author = value; } }
		public string? Genre { get => _genre; private set { _genre = value; } }
		public string? Description { get => _description; private set { _description = value; } }
		public string? Image { get => _image; private set { _image = value; } }
		public int Date { get => _date; private set { _date = value; } }
		public byte[]? ByteImage { get => _byteImage; private set { _byteImage = value; } }
		public int MaxCount { get => _maxCount; private set { _maxCount = value; } }
		public long Barcode { get => _barcode; private set { _barcode = value; } }

		public Book(int id, string title, string? author, string? genre,
			string? description, string image, int date, int maxCount, long barcode)
		{
			Id = id;
			Title = title;
			Author = author;
			Genre = genre;
			Description = description;
			Image = image;
			Date = date;
			MaxCount = maxCount;
			Barcode = barcode;
		}

		public Book(int id, string title, string? author, string? genre,
			string? description, byte[] image, int date, int maxCount, long barcode)
		{
			Id = id;
			Title = title;
			Author = author;
			Genre = genre;
			Description = description;
			ByteImage = image;
			Date = date;
			MaxCount = maxCount;
			Barcode = barcode;
		}

		public void ChangeDescriptionForSql()
		{
			Description = Description.Replace("\'", "\'\'");
			/*Description = Description.Replace("\"", "\"\"");*/
		}
	}
}
