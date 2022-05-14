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
	public partial class StudentsListWindow : Window, IDisposable
	{
		private MsSqlController _connection = new MsSqlController();

		private List<Student> _students = new List<Student>();
		private List<RentRow> _rentRows = new List<RentRow>();

		private SqlConnection _conn;

		private SqlConnection _sqlConnection;

		private int _bookId;

		private object _currentStudent;

		public StudentsListWindow(int bookId)
		{
			InitializeComponent();

			_bookId = bookId;

			_connection.FillRentList(_bookId);

			StudentsDataGrid.ItemsSource = _connection.AllRents; /*new ObservableCollection<RentRow>();*/

			_students = _connection.AllStudents;
			_rentRows = _connection.AllRents;
			/*
						string sql = "SELECT * FROM students";
						*//*using (SqlConnection connection = *//* _conn = new SqlConnection(MsSqlController.ConnectionString);
						//{
							*//*connection*//*_conn.Open();

							// Создаем объект DataAdapter
							*//*SqlDataAdapter*//* adapter = new SqlDataAdapter(sql, _conn);

							// Создаем объект Dataset
							*//*DataSet ds = new DataSet();*//*

							// Заполняем Dataset
							adapter.Fill(ds);

							// Отображаем данные
							StudentsDataGrid.ItemsSource = ds.Tables[0].DefaultView;
						//}
			*/
		}

		private void AllStudentsButton_Click(object sender, RoutedEventArgs e)
		{
			var sqlController = new MsSqlController();

			if (AllStudentsButton.Content == "Rent List")
			{
				AddSelectedButton.Visibility = Visibility.Hidden;

				sqlController.FillRentList(_bookId);

				StudentsDataGrid.ItemsSource = sqlController.AllRents;

				AllStudentsButton.Content = "Add Students";
			}
			else
			{
				StudentsDataGrid.Columns[0].Visibility = Visibility.Visible;
				AddSelectedButton.Visibility = Visibility.Visible;

				StudentsDataGrid.ItemsSource = sqlController.AllStudents;

				AllStudentsButton.Content = "Rent List";
			}
		}

		private /*async*/ void StudentsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
		{
			if (e.Row.DataContext is Student)
			{
				if ((e.Row.DataContext as Student) is null) MessageBox.Show("New Row!");

				_currentStudent = StudentsDataGrid.SelectedItem;

				new Thread(()=>
				{
					Dispatcher.BeginInvoke((Action)delegate { BindRowValue(); });
				}).Start();

				/*await Task.Run(() => BindRowValue());*/
			}
		}

		private async void BindRowValue()
		{
			/*Thread.Sleep(1000);*/

			await Task.Delay(1000);

			MessageBox.Show($"{((Student)_currentStudent).Name}");

			if (((Student)_currentStudent).Id == 0)
			{
				using (var connection = new SqlConnection(MsSqlController.ConnectionString))
				{
					connection.Open();

					var command = new SqlCommand("use librarydb; " +
						"insert into students " +
						$"values ('{((Student)_currentStudent).Name}', {((Student)_currentStudent).FailedDeadlines})", connection);

					command.ExecuteNonQuery();

					/*command.CommandText = "select ident_current('students') + 1";

					using (var reader = command.ExecuteReader())
					{
						
					}*/
				}
			}
			else
			{
				using (var connection = new SqlConnection(MsSqlController.ConnectionString))
				{
					connection.Open();

					var command = new SqlCommand("use librarydb; " +
						"update students " +
						$"set " +
						$"name = '{((Student)_currentStudent).Name}', " +
						$"failed_deadlines = {((Student)_currentStudent).FailedDeadlines} " +
						$"where id = {((Student)_currentStudent).Id}", connection);

					command.ExecuteNonQuery();
				}
			}

			Update();
		}

		private void Update()
		{
			StudentsDataGrid.ItemsSource = new MsSqlController().AllStudents;
		}

		private void StudentsDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				MessageBox.Show(GetStringRange());

				using (var connection = new SqlConnection(MsSqlController.ConnectionString))
				{
					connection.Open();

					var command = new SqlCommand("use librarydb; " +
						"delete students " +
						$"where id in ({GetStringRange()})" /*{((Student)StudentsDataGrid.SelectedItem).Id}*/, connection);

					command.ExecuteNonQuery();
				}

				StudentsDataGrid.SelectedItem = new MsSqlController().AllStudents;
			}
		}

		private string GetStringRange()
		{
			var selectedItems = StudentsDataGrid.SelectedItems;

			string result = "";

			for (int i = 0; i < selectedItems.Count; i++)
			{
				result += i == 0 ? $"{((Student)selectedItems[0]).Id}" : $", {((Student)selectedItems[i]).Id}";
			}

			return result;
		}

		private void AddSelectedButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(StudentsDataGrid.SelectedItems.Count.ToString());

			if (StudentsDataGrid.SelectedItems.Count > 0)
			{
				using (var connection = new SqlConnection(MsSqlController.ConnectionString))
				{
					connection.Open();

					var command = new SqlCommand("use librarydb; " +
						"insert into student_book_connection (book_id, student_id, rent_date, deadline, is_deadline_failed) " +
						"select books.id, students.id, GETDATE(), GETDATE() + 14, 0 " +
						"from books, students " +
						$"where books.id = {_bookId} and students.id in ({GetStringRange()});", connection);

					command.ExecuteNonQuery();
				}
			}
		}

		public void Dispose()
		{
			_conn.Close();

			_sqlConnection.Close();
		}
	}
}
