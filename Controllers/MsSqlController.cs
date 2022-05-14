using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace LibraryNET6Pages
{
    public class MsSqlController : IDisposable
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

        public MsSqlController(Type sender = null)
        {
            _bookList = new List<Book>();
            _genreList = new List<Genre>();
            _rentList = new List<RentRow>();
            _studentList = new List<Student>();

            _connection = new SqlConnection(sender is not null ? _AdminConnectionString : _AssistantConnectionString);
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
                FillStudentList();
            }
        }

        public static Tuple<bool, bool> IsEnterDataCorrect(string login, string password)
		{
            bool isLoginExists = false;
            bool isPasswordCorrect = true;

            string sqlConnection = $"Server=(local);Integrated Security=false;Database=librarydb;User ID={login};Password={password};Trust Server Certificate=True;";

            using (var connection = new SqlConnection(_AdminConnectionString))
			{
                connection.Open();

                var sqlCommand = new SqlCommand($"use librarydb; select is_srvrolemember('sysadmin', '{login}');", connection);

                using (var reader = sqlCommand.ExecuteReader())
				{
                    if (reader.Read())
					{
                        isLoginExists = Convert.ToBoolean(int.Parse(reader.GetValue(0).ToString()));
					}
				}

                if (isLoginExists)
                {
                    connection.Close();

                    connection.ConnectionString = sqlConnection;

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

            return Tuple.Create(isLoginExists, isPasswordCorrect);
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
                if (book.Title.ToLower().Contains(query.ToLower()))
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
                 "@date int " +
                 "" +
                 $"set @title = '{book.Title}' " +
                 $"set @author = '{book.Author}' " +
                 $"set @genre = '{book.Genre}' " +
                 $"set @description = '{book.Description}' " +
                 $"set @date = {book.Date} " +
                 "" +
                 "exec sp_fill_lib_data @title, @author, @genre, @description, @image, @date;"
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
                 "@date int " +
                 "" +
                 $"set @id = {book.Id} " +
                 $"set @title = '{book.Title}' " +
                 $"set @author = '{book.Author}' " +
                 $"set @genre = '{book.Genre}' " +
                 $"set @description = '{book.Description}' " +
                 $"set @date = {book.Date} " +
                 "" +
                 "exec sp_EditBook @id, @title, @author, @genre, @description, @image, @date;"
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

        public string ExecuteCommand(SqlCommand sqlCommand)
        {
            try
            {
                /*using (var connection = new SqlConnection(_ConnectionString))
                {
                    connection.Open();
*/
                sqlCommand.Connection = _connection;

                sqlCommand.ExecuteNonQuery();
                //}
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
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
                            short.Parse(reader.GetValue(0).ToString()),         //  id
                            reader.GetValue(1).ToString(),                   //  title
                            reader.GetValue(2).ToString(),                  //  author
                            reader.GetValue(3).ToString(),                   //  genre
                            reader.GetValue(4).ToString(),             //  description
                            Convert.ToBase64String(byteImage),               //  image
                            short.Parse(reader.GetValue(6).ToString())        //  date
                        ));
                    }
                }
            }
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

        public void AddStudent(Student newStudent)
		{
            string sqlCommand = "insert into students " +
                $"values ('{newStudent.Name}', {newStudent.FailedDeadlines})";

            new SqlCommand(sqlCommand, _connection).ExecuteNonQuery();
		}

        public void EditStudent(Student currentStudent)
		{
            string sqlCommand = "update students " +
                "set " +
                $"name = '{currentStudent.Name}', " +
                $"failed_deadlines = {currentStudent.FailedDeadlines} " +
				$"where id = {currentStudent.Id}";

            new SqlCommand(sqlCommand, _connection).ExecuteNonQuery();
		}
        
        public void DeleteStudent(int studentId)
		{
            string sqlCommand = "delete students " +
                $"where id = {studentId}";

            new SqlCommand(sqlCommand, _connection).ExecuteNonQuery();
		}

        public void FillRentList(int bookId)
        {
            string sqlCommand =
                "use librarydb; " +
                "select students.name as student, rent_date, deadline, return_date, is_deadline_failed " +
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
						{
                            Student = reader.GetValue(0).ToString(),
                            RentDate = rentDate/* == DateTime.MinValue ? DateTime.Now : rentDate*/,
                            Deadline = deadline/* == DateTime.MinValue ? DateTime.Now.AddDays(14) : deadline*/,
                            /*RentDate = DateTime.Parse(reader.GetValue(1).ToString()),
                            Deadline = DateTime.Parse(reader.GetValue(2).ToString()),*/
							ReturnDate = returnDate == DateTime.MinValue ? null : returnDate,
							IsDeadlineFailed = isDeadlineFailed
                        });

                        _rentList[0].ReturnDate.ToString();
					}
                }
            }
        }

        private void FillStudentList()
        {
            string sqlCommand = "select * from students";

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

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
