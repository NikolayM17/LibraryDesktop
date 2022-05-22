using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace LibraryNET6Pages
{
    public class MsSqlController<T> : IDisposable
        where T : Page
    {
        private List<Book> _bookList;
        private List<Genre> _genreList;
        private List<RentRow> _rentList;
        private List<Student> _studentList;

        private const string _DefaultSqlCommand = "select * from lib_view";
        private const string _AdminConnectionString =
            @"Server=(local);Integrated Security=false;Database=librarydb;User ID=libadmin;Password=libadmin;Trust Server Certificate=True;";
        private const string _AssistantConnectionString =
            @"Server=(local);Integrated Security=false;Database=librarydb;User ID=assistant;Password=assistant;Trust Server Certificate=True;";

        public string User { get; set; }

        public static string ConnectionString { get { return _AdminConnectionString; } }

        private SqlConnection _connection;

        public MsSqlController()
        {
            _bookList = new List<Book>();
            _genreList = new List<Genre>();
            _rentList = new List<RentRow>();
            _studentList = new List<Student>();

            _connection = new SqlConnection(typeof(T) == typeof(AdminPage) ? _AdminConnectionString : _AssistantConnectionString);
            {
                _connection.Open();

                using (var reader = new SqlCommand("select current_user;", _connection).ExecuteReader())
				{
                    if (reader.Read())
					{
                        User = reader.GetValue(0).ToString();
					}
				}
                
                FillBookList(_DefaultSqlCommand);
                FillGenreList();
                /*FillStudentList();*/
            }
        }

        public static Tuple<bool, bool> IsEnterDataCorrect(string login, string password)
		{
            bool isLoginExists = false;
            bool isUserAdmin = false;
            bool isPasswordCorrect = true;

            using (var connection = new SqlConnection(_AdminConnectionString))
			{
                connection.Open();

                var sqlCommand = new SqlCommand($"select count(loginname) from librarydb.sys.syslogins where loginname = '{login}';", connection);

                using (var reader = sqlCommand.ExecuteReader())
				{
                    if (reader.Read())
					{
                        isLoginExists = int.Parse(reader.GetValue(0).ToString()) > 0;
					}
				}

                if (isLoginExists)
                {
                    sqlCommand = new SqlCommand($"select is_srvrolemember('sysadmin', '{login}');", connection);

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isUserAdmin = Convert.ToBoolean(int.Parse(reader.GetValue(0).ToString()));
                        }
                    }
                }

                if (isUserAdmin)
                {
                    connection.Close();

                    connection.ConnectionString =
                        $"Server=(local);Integrated Security=false;Database=librarydb;User ID={login};Password={password};Trust Server Certificate=True;";

                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        isPasswordCorrect = false;
                    }
                }

			}

            return Tuple.Create(isUserAdmin, isPasswordCorrect);
		}

        public List<Book> AllBooks { get => _bookList; }
        public List<string> AllGenres
        {
            get
            {
                var genres = new List<string>();

                foreach (var genre in _genreList)
                {
                    genres.Add(genre.Name);
                }

                return genres;
            }
        }
        public List<RentRow> AllRents { get => _rentList; }
        public List<Student> AllStudents { get => _studentList; }

        public List<Book> GetFoundBooks(string query)
        {
            var foundBooks = new List<Book>();

            foreach (var book in _bookList)
            {
                if (book.Title.ToLower().Contains(query.ToLower()) ||
                    book.Author.ToLower().Contains(query.ToLower()) ||
                    book.Genre.ToLower().Contains(query.ToLower()) ||
                    book.Barcode.ToString().ToLower().Contains(query.ToLower()))
                {
                    foundBooks.Add(book);
                }
            }

            return foundBooks;
        }

        public string AddBook(Book book)
        {
            book.ChangeDescriptionForSql();

            var command = new SqlCommand(
                 "use librarydb; " +
                 "declare " +
                 "@title varchar(100), " +
                 "@author varchar(30), " +
                 "@genre varchar(30), " +
                 "@description varchar(1000), " +
                 "@date int," +
				 "@count int, " +
				 "@barcode bigint " +
                 "" +
                 $"set @title = '{book.Title}' " +
                 $"set @author = '{book.Author}' " +
                 $"set @genre = '{book.Genre}' " +
                 $"set @description = '{book.Description}' " +
                 (book.Date < 0 ? "" : $"set @date = {book.Date}, ") +
				 (book.MaxCount < 0 ? "" : $"set @count = {book.MaxCount} ") +
                 (book.Barcode < 0 ? "" : $"set @barcode = {book.Barcode} ") +
                 "" +
                 "exec sp_fill_lib_data @title, @author, @genre, @description, @image, @date, @count, @barcode;"
                 );

            var sqlParameter = new SqlParameter("@image", SqlDbType.VarBinary);

            sqlParameter.Value = book.ByteImage;

            command.Parameters.Add(sqlParameter);

            return ExecuteCommand(command);
        }

        public string EditBook(Book book)
        {
            book.ChangeDescriptionForSql();

            var command = new SqlCommand(
                 "use librarydb; " +
                 "declare " +
                 "@id int, " +
                 "@title varchar(100), " +
                 "@author varchar(30), " +
                 "@genre varchar(30), " +
                 "@description varchar(1000), " +
                 "@date int, " +
				 "@count int, " +
				 "@barcode bigint " +
                 "" +
                 $"set @id = {book.Id} " +
                 $"set @title = '{book.Title}' " +
                 $"set @author = '{book.Author}' " +
                 $"set @genre = '{book.Genre}' " +
                 $"set @description = '{book.Description}' " +
                 $"set @date = {book.Date} " +
				 $"set @count = {book.MaxCount} + " +
				 "(select isnull((select count(student_id) as count " +
				 "from student_book_connection " +
				 $"where book_id = {book.Id} " +
				 "group by book_id), 0)) " +
				 $"set @barcode = {book.Barcode} " +
                 "" +
                 "exec sp_EditBook @id, @title, @author, @genre, @description, @image, @date, @count, @barcode;"
                 );

            var sqlParameter = new SqlParameter("@image", SqlDbType.VarBinary);

            sqlParameter.Value = book.ByteImage; /*Convert.FromBase64String(book.Image);*/

            command.Parameters.Add(sqlParameter);

            return ExecuteCommand(command);
        }

        public string DeleteBook(Book book)
        {
            var command = new SqlCommand(
                "use librarydb; " +
                "delete books " +
                $"where id = {book.Id}");

            return ExecuteCommand(command);
        }

        public static int GetRemainingCount(int bookId)
		{
            if (typeof(T) == typeof(AdminPage))
            {
                string command = 
                "select max_count - (" +
                "select isnull((select count(student_id) as count " +
                "from student_book_connection " +
                $"where book_id = {bookId} " +
                "group by book_id), 0)) as count " +
                "" +
                "from books " +
				$"where id = {bookId}";

                using (var connection = new SqlConnection(_AdminConnectionString))
                {
                    connection.Open();

                    using (var reader = new SqlCommand(command, connection).ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                return int.Parse(reader.GetValue(0).ToString());
                            }
                        }

                        return 0;
                    }
                }
            }

            return 0;
        }

        /*public static int GetBookMaxCount(int bookId)
        {

        }*/

        private string ExecuteCommand(SqlCommand sqlCommand)
        {
            try
            {
                sqlCommand.Connection = _connection;

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Executed.";
        }

        private void FillBookList(string sqlCommand = _DefaultSqlCommand)
        {
            byte[] byteImage = new byte[0];

            var newCommand = new SqlCommand(sqlCommand, _connection);

            using (SqlDataReader reader = newCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byteImage = new byte[reader.GetBytes(5, 0, null, 0, int.MaxValue) - 1];
                        reader.GetBytes(5, 0, byteImage, 0, byteImage.Length);

                        _bookList.Add(new Book
                        (
                            int.Parse(reader.GetValue(0).ToString()),           //  id
                            reader.GetValue(1).ToString(),                   //  title
                            reader.GetValue(2).ToString(),                  //  author
                            reader.GetValue(3).ToString(),                   //  genre
                            reader.GetValue(4).ToString(),             //  description
                            Convert.ToBase64String(byteImage),               //  image
                            int.Parse(reader.GetValue(6).ToString().Length == 0 ?
                            "-1" : reader.GetValue(6).ToString()),            //  date
                            int.Parse(reader.GetValue(7).ToString().Length == 0 ?
                            "-1" : reader.GetValue(7).ToString()),           //  count
                            long.Parse(reader.GetValue(8).ToString().Length == 0 ?
                            "-1" : reader.GetValue(8).ToString())          //  barcode
                        ));
                    }
                }
            }
        }

        public void UpdateBookList()
		{
            _bookList.Clear();
            FillBookList();
		}

        private void FillGenreList()
        {
            string sqlCommand = "select * from genres";

            var newCommand = new SqlCommand(sqlCommand, _connection);

            using (SqlDataReader reader = newCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
					{
						_genreList.Add(new Genre(
							short.Parse(reader.GetValue(0).ToString()),
							reader.GetValue(1).ToString()
							));
					}
                }
            }
        }
        
        public void FillRentList(int bookId)
        {
            _rentList.Clear();

            string sqlCommand =
                "use librarydb; " +
                "select students.name as student, rent_date, deadline, return_date, is_deadline_failed, student_id " +
				"from student_book_connection " +
				"inner join students on student_book_connection.student_id = students.id " +
                $"where book_id = {bookId};";

            var newCommand = new SqlCommand(sqlCommand, _connection);

            using (SqlDataReader reader = newCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
					{
						/*var returnDate = new DateTime();*/

						/*returnDate = Convert.ToDateTime(reader.GetValue(0).ToString());*/

						DateTime.TryParse(reader.GetValue(1).ToString(), out DateTime rentDate);
						DateTime.TryParse(reader.GetValue(2).ToString(), out DateTime deadline);
						DateTime.TryParse(reader.GetValue(3).ToString(), out DateTime returnDate);
                        bool.TryParse(reader.GetValue(4).ToString(), out bool isDeadlineFailed);

                        _rentList.Add(new RentRow
						(
                            id: int.Parse(reader.GetValue(5).ToString()),
                            name: reader.GetValue(0).ToString(),
                            rentDate:  rentDate/* == DateTime.MinValue ? DateTime.Now : rentDate*/,
                            deadline: deadline/* == DateTime.MinValue ? DateTime.Now.AddDays(14) : deadline*/,
                            /*RentDate = DateTime.Parse(reader.GetValue(1).ToString()),
                            Deadline = DateTime.Parse(reader.GetValue(2).ToString()),*/
							returnDate: (returnDate == DateTime.MinValue || returnDate == (DateTime.Parse("1/1/1900 12:00:00 AM"))) ? null : returnDate,
                            isDeadlineFailed: isDeadlineFailed
                        ));

                        /*_rentList[0].ReturnDate.ToString();*/
					}
                }
            }
        }

        public string DeleteRentRange(string rangeStudentId, int bookId)
		{
            var command = new SqlCommand(
                "delete student_book_connection " +
                $"where student_id in ({rangeStudentId}) and book_id = {bookId}");

            return ExecuteCommand(command);
        }

        public string EditRentRow(RentRow rentRow, int bookId)
		{
            var command = new SqlCommand(
                "update student_book_connection " +
                "set " +
                $"rent_date = '{rentRow.RentDate.ToString()}', " +
                $"deadline = '{rentRow.Deadline.ToString()}', " +
                $"return_date = '{rentRow.ReturnDate.ToString()}', " +
                $"is_deadline_failed = {Convert.ToInt32(rentRow.IsDeadlineFailed)} " +
                $"where book_id = {bookId} and student_id = {rentRow.Id}");

            return ExecuteCommand(command);
		}

        public string AddRentRange(string rangeStudentId, int bookId)
		{
            var command = new SqlCommand(
                "insert into student_book_connection (book_id, student_id, rent_date, deadline, is_deadline_failed) " +
                "select books.id, students.id, GETDATE(), GETDATE() + 14, 0 " +
                "from books, students " +
                $"where books.id = {bookId} and students.id in ({rangeStudentId});");

            return ExecuteCommand(command);
        }

        public void FillStudentList(int ignore_id)
        {
            _studentList.Clear();

            string sqlCommand =
                "select * from students " +
				"where id not in (" +
				"select id from students " +
				"inner join student_book_connection on students.id = student_book_connection.student_id " +
				$"where book_id = {ignore_id})";

            var newCommand = new SqlCommand(sqlCommand, _connection);

            using (SqlDataReader reader = newCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _studentList.Add(new Student
                        (
                            id: short.Parse(reader.GetValue(0).ToString()),
                            name: reader.GetValue(1).ToString(),
                            failed_deadlines: short.Parse(reader.GetValue(2).ToString())
                        ));
                    }
                }
            }
        }

        public string DeleteStudentRange(string rangeStudentId, int bookId)
		{
            var command = new SqlCommand(
                "delete student " +
                $"where student_id in ({rangeStudentId}) and book_id = {bookId}");

            return ExecuteCommand(command);
		}

        public string EditStudent(Student student)
		{
            var command = new SqlCommand(
                "update students " +
                $"set " +
                $"name = '{student.Name}', " +
                $"failed_deadlines = {student.FailedDeadlines} " +
                $"where id = {student.Id}");

            return ExecuteCommand(command);
        }

        public string AddStudent(Student newStudent)
		{
            var command = new SqlCommand(
                "insert into students " +
                $"values ('{newStudent.Name}', {newStudent.FailedDeadlines})");

            return ExecuteCommand(command);
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
