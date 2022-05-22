using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryNET6Pages
{
	/// <summary>
	/// Логика взаимодействия для StudentsListWindow.xaml
	/// </summary>
	public partial class StudentsListWindow : Window
		/*, IDisposable*/
		/*where T : AdminPage, new()*/
	{
		/*private MsSqlController _connection = new MsSqlController();*/

		/*private List<Student> _students = new List<Student>();
		private List<RentRow> _rentRows = new List<RentRow>();

		private SqlConnection _conn;
		private SqlConnection _sqlConnection;*/

		private int _bookId;

		private object _currentSelected;

		private MsSqlController<AdminPage> _librarydb;

		public StudentsListWindow(int bookId)
		{
			InitializeComponent();

			_bookId = bookId;
			_librarydb = new();

			_librarydb.FillRentList(_bookId);

			/*_connection.FillRentList(_bookId);
			_connection.FillStudentList(_bookId);

			_students = _connection.AllStudents;
			_rentRows = _connection.AllRents;

			StudentsDataGrid.ItemsSource = _connection.AllRents;*/

			StudentsDataGrid.ItemsSource = _librarydb.AllRents;
			RemainingCountTextBox.Text = _librarydb.AllBooks.FirstOrDefault(x => x.Id == _bookId).MaxCount.ToString();
			AddSelectedButton.Visibility = Visibility.Hidden;
		}
		
		private void AllStudentsButton_Click(object sender, RoutedEventArgs e)
		{
			/*var sqlController = new MsSqlController();*/

			if (AllStudentsButton.Content == "Rent List")
			{
				AddSelectedButton.Visibility = Visibility.Hidden;

				_librarydb.FillRentList(_bookId);
				StudentsDataGrid.ItemsSource = _librarydb.AllRents;

				AllStudentsButton.Content = "Add Students";
			}
			else
			{
				AddSelectedButton.Visibility = Visibility.Visible;

				_librarydb.FillStudentList(_bookId);
				StudentsDataGrid.ItemsSource = _librarydb.AllStudents;

				AllStudentsButton.Content = "Rent List";
			}

			/*
			switch(AllStudentsButton.Content.ToString())
			{
				case "Add Student":

					sqlController.FillStudentList(_bookId);
					StudentsDataGrid.ItemsSource = sqlController.AllStudents;

					AddSelectedButton.Visibility = Visibility.Visible;
					AllStudentsButton.Content = "Rent List";

					break;

				case "Rent List":

					sqlController.FillRentList(_bookId);
					StudentsDataGrid.ItemsSource = sqlController.AllRents;

					AddSelectedButton.Visibility = Visibility.Hidden;
					AllStudentsButton.Content = "Add Students";

					break;
			}
			*/

			/*
			if (AllStudentsButton.Content.ToString() == "Rent List")
			{
				AddSelectedButton.Visibility = Visibility.Hidden;

				sqlController.FillRentList(_bookId);

				StudentsDataGrid.ItemsSource = sqlController.AllRents;

				AllStudentsButton.Content = "Add Students";
			}
			else if ()
			{
				AddSelectedButton.Visibility = Visibility.Visible;

				sqlController.FillStudentList(_bookId);

				StudentsDataGrid.ItemsSource = sqlController.AllStudents;

				AllStudentsButton.Content = "Rent List";

				//	StudentsDataGrid.CanUserAddRows = true;
			}*/
		}

		private void StudentsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
		{
			_currentSelected = StudentsDataGrid.SelectedItem;

			if (e.Row.DataContext is Student)
			{
				if (((Student)e.Row.DataContext).Id == 0) MessageBox.Show("New Row!");

				new Thread(() =>
				{
					Dispatcher.BeginInvoke((Action)delegate { BindRowValue("students"); });
				}).Start();

				/*await Task.Run(() => BindRowValue());*/
			}
			else if (e.Row.DataContext is RentRow)
			{
				if (((RentRow)e.Row.DataContext).Id == 0) MessageBox.Show("New Row!");

				new Thread(() =>
				{
					Dispatcher.BeginInvoke((Action)delegate { BindRowValue("student_book_connection"); });
				}).Start();
			}
		}

		private async void BindRowValue(string tableName)
		{
			/*Thread.Sleep(1000);*/

			await Task.Delay(1000);

			if (tableName == "students")
			{
				var currentStudent = (Student)_currentSelected;

				if (currentStudent.Id == 0)
				{
					/*var message = */_librarydb.AddStudent(currentStudent);

					/*MessageBox.Show(message is not null ? message : "Done");*/

					/*using (var connection = new SqlConnection(MsSqlController.ConnectionString))
					{
						connection.Open();

						var command = new SqlCommand("use librarydb; " +
							"insert into students " +
							$"values ('{currentStudent.Name}', {currentStudent.FailedDeadlines})", connection);

						command.ExecuteNonQuery();
					}*/
				}
				else
				{
					/*var message = */_librarydb.EditStudent(currentStudent);

					/*MessageBox.Show(message is not null ? message : "Done");*/

					/*using (var connection = new SqlConnection(MsSqlController.ConnectionString))
					{
						connection.Open();

						var command = new SqlCommand("use librarydb; " +
							"update students " +
							$"set " +
							$"name = '{currentStudent.Name}', " +
							$"failed_deadlines = {currentStudent.FailedDeadlines} " +
							$"where id = {currentStudent.Id}", connection);

						command.ExecuteNonQuery();
					}*/
				}

				UpdateStudentTable();
			}
			else if (tableName == "student_book_connection")
			{
				var currentRentRow = (RentRow)_currentSelected;

				/*var message = */_librarydb.EditRentRow(currentRentRow, _bookId);

				/*MessageBox.Show(message is not null ? message : "Done");*/

				UpdateRowRentTable();

				/*if (currentRentRow.Id == 0)
				{
					using (var connection = new SqlConnection(MsSqlController.ConnectionString))
					{
						connection.Open();

						var command = new SqlCommand("use librarydb; " +
							"insert into student_book_connection " +
							$"values ('{currentRentRow.Student}', {currentRentRow.RentDate}, {currentRentRow.Deadline}, {currentRentRow.ReturnDate}, {Convert.ToInt32(currentRentRow.IsDeadlineFailed)})", connection);

						command.ExecuteNonQuery();
					}
				}
				else
				{*/


				/*
				using (var connection = new SqlConnection(MsSqlController.ConnectionString))
				{

					connection.Open();

					var command = new SqlCommand(
						"use librarydb; " +
						"" +
						"update student_book_connection " +
						"set " +
						$"rent_date = '{currentRentRow.RentDate.ToString()}', " +
						$"deadline = '{currentRentRow.Deadline.ToString()}', " +
						$"return_date = '{currentRentRow.ReturnDate.ToString()}', " +
						$"is_deadline_failed = {Convert.ToInt32(currentRentRow.IsDeadlineFailed)} " +
						$"where book_id = {_bookId} and student_id = {currentRentRow.Id}", connection);*/

				/*"use librarydb; " +
				"update student_book_connection " +
				$"set " +
				$"rent_date = {currentRentRow.ReturnDate}, " +
				$"deadline = {currentRentRow.Deadline}, " +
				$"return_date = {currentRentRow.ReturnDate}, " +
				$"is_deadline_failed = {Convert.ToBoolean(currentRentRow.IsDeadlineFailed)} " +
				$"where book_id = {_bookId} and student_id = {currentRentRow.Id}", connection);

				command.ExecuteNonQuery();
				}
				*/
			}
		}

		private void UpdateStudentTable()
		{
			_librarydb.FillStudentList(_bookId);

			StudentsDataGrid.ItemsSource = new List<Student>(_librarydb.AllStudents);

			RemainingCountTextBox.Text = MsSqlController<AdminPage>.GetRemainingCount(_bookId).ToString();
		}

		private void UpdateRowRentTable()
		{
			_librarydb.FillRentList(_bookId);

			StudentsDataGrid.ItemsSource = new List<RentRow>(_librarydb.AllRents);

			RemainingCountTextBox.Text = MsSqlController<AdminPage>.GetRemainingCount(_bookId).ToString();
		}

		private void StudentsDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				if (AllStudentsButton.Content.ToString() == "Add Students")
				{
					/*var message = */_librarydb.DeleteRentRange(GetStringRange("student_book_connection"), _bookId);

					/*MessageBox.Show(message is not null ? message : "Done");*/

					UpdateRowRentTable();

					/*var sql = new MsSqlController(typeof(AdminPage));

					sql.FillRentList(_bookId);
					
					MessageBox.Show(sql.DeleteRentRange(GetStringRange("student_book_connection"), _bookId));*/

					/*
					using (var connection = new SqlConnection(MsSqlController.ConnectionString))
					{
						connection.Open();

						var command = new SqlCommand("use librarydb; " +
							"delete student_book_connection " +
							$"where student_id in ({GetStringRange("rentRow")}) and book_id = {_bookId}", connection);

						command.ExecuteNonQuery();
					}*/

					/*sql = new MsSqlController();*/

					/*sql.FillRentList(_bookId);

					StudentsDataGrid.ItemsSource = sql.AllRents;*/
				}
				else if (AllStudentsButton.Content.ToString() == "Rent List")
				{
					AddRents();
				}
			}
		}

		private string GetStringRange(string tableName)
		{
			var selectedItems = StudentsDataGrid.SelectedItems;

			string result = "";

			for (int i = 0; i < selectedItems.Count; i++)
			{
				int id = tableName == "students" ?
					((Student)selectedItems[i]).Id : ((RentRow)selectedItems[i]).Id;

				result += i == 0 ? $"{id}" : $", {id}";
			}

			return result;
		}

		private void AddSelectedButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(GetStringRange("students"));

			AddRents();
		}

		private void AddRents()
		{
			if (StudentsDataGrid.SelectedItems.Count > 0)
			{
				if (StudentsDataGrid.SelectedItems.Count <= int.Parse(RemainingCountTextBox.Text))
				{
					/*var message = */_librarydb.AddRentRange(GetStringRange("students"), _bookId);

					/*MessageBox.Show(message is not null ? message : "Done");*/
				}
				else
				{
					MessageBox.Show("Книг в наличии больше нет!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			}

			UpdateStudentTable();
		}

		private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			((DataGridBoundColumn)e.Column).Binding.TargetNullValue = string.Empty;

			/*if (AllStudentsButton.Content.ToString() == "All Students")*/

			if (e.PropertyType == typeof(System.DateTime))
			{
				(e.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy HH:mm:ss";
			}
		}
	}
}
