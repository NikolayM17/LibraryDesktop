using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryNET6Pages
{
	/// <summary>
	/// Логика взаимодействия для AdminPage.xaml
	/// </summary>
	public partial class AdminPage : Page
	{
		private MsSqlController _librarydb;

		public AdminPage()
		{
			InitializeComponent();

			_librarydb = new MsSqlController(this.GetType());

			/*MessageBox.Show(_librarydb.User);*/

			/*var allBooks = new MsSqlController().AllBooks;*/

			SearchResultsStackPanel.Children.Clear();

			DisplayFoundBooks(_librarydb.AllBooks, this);
		}

		private void DisplayFoundBooks(List<Book> foundBooks, Page page)
		{
			var borderList = new List<Border>();

			var listBorderLists = new List<List<Border>>();

			var gridBooks = new List<Book>();

			var listGridBooks = new List<List<Book>>();

			foreach (var book in foundBooks)
			{
				gridBooks.Add(book);

				if (gridBooks.Count == 4)
				{
					listGridBooks.Add(gridBooks);
					gridBooks = new List<Book>();
				}
			}

			if (gridBooks.Count > 0)
			{
				listGridBooks.Add(gridBooks);
			}

			foreach (var fourBooks in listGridBooks)
			{
				foreach (var book in fourBooks)
				{
					var gridController = new GridController(this);

					var newBorder = gridController.CreateBorder(foundBooks.IndexOf(book));

					borderList.Add(gridController.FillBorder(
						gridController.CreateBorder(
							fourBooks.IndexOf(book)), book));
				}

				listBorderLists.Add(borderList);

				borderList = new List<Border>();
			}

			foreach (var fourBorders in listBorderLists)
			{
				var gridController = new GridController(this);

				var grid = gridController.FillGrid(gridController.CreateGrid(), fourBorders);

				grid.UpdateLayout();

				SearchResultsStackPanel.Children.Add(grid);

				SearchResultsStackPanel.UpdateLayout();
			}
		}

		private void Main_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new MainFrame());
		}

		private void AddBook_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new AddBookPage());



			/*new SqliteController().ExecuteCommand(new SqliteCommand(
				"INSERT INTO books (name, author, genre, description, year)" +
				"VALUES('Гарри Поттер и философский камень'," +
				"'Дж. К. Роулинг',"+
				"'Зарубежное фэнтези'," +
				"'Гарри Поттер ни разу даже не слышал о «Хогварце», но на дверной коврик дома номер четыре по Бирючинной улице начинают падать письма. Адрес написан зелеными чернилами на желтоватом пергаменте, а конверт скрепляет лиловая печать. Однако письма тут же конфисковывают тетя и дядя мальчика, имеющие на редкость скверный характер. Потом, на одиннадцатый день рождения Гарри, в дом врывается гигант по имени Рубеус Огрид с невероятными новостями: Гарри Поттер – волшебник, и его ждет место в школе колдовства и ведьминских искусств «Хогварц». Потрясающие приключения начинаются!'," +
				"1997)"
				));*/
		}

		private void DeleteBook_Click(object sender, RoutedEventArgs e)
		{
			/*new SqliteController().ExecuteCommand(new SqliteCommand(
				"DELETE FROM books " +
				"WHERE books.id == 1"
				));*/
		}

		private void EditBook_Click(object sender, RoutedEventArgs e)
		{
			/*new SqliteController().ExecuteCommand(new SqliteCommand(
				"UPDATE books " +
				"SET year = 2000 " +
				"WHERE year == 2017"
			));*/
		}

		private void EditBook_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void EditBook_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void ContextMenu_MouseEnter(object sender, MouseEventArgs e)
		{
			/*((MenuItem)((ContextMenu)sender).Items[((ContextMenu)sender).Items.Count - 1]).Background = new SolidColorBrush(Colors.White);*/
		}


		private void SearchTextBox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 255));
		}

		private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			SearchWatermark.Visibility = Visibility.Collapsed;
		}

		private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (((TextBox)sender).Text.Length == 0)
			{
				SearchWatermark.Visibility = Visibility.Visible;
			}
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			/*var foundBooks = new MsSqlController().GetFoundBooks(SearchTextBox.Text);*/

			SearchResultsStackPanel.Children.Clear();

			DisplayFoundBooks(_librarydb.GetFoundBooks(SearchTextBox.Text), this);
		}

		private void Page_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SearchButton_Click(sender, e);
			}
		}

		private void SearchButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			SearchImage.Source = new BitmapImage(new Uri(@"/assets/search_pressed.png", UriKind.Relative));
		}

		private void SearchButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			SearchImage.Source = new BitmapImage(new Uri(@"/assets/search.png", UriKind.Relative));
		}

		private void AddBookButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			AddBookImage.Source = new BitmapImage(new Uri(@"/assets/add_icon_pressed.png", UriKind.Relative));

			NavigationService.Navigate(new AddBookPage());
		}

		private void AddBookButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			AddBookImage.Source = new BitmapImage(new Uri(@"/assets/add_icon.png", UriKind.Relative));
		}
	}
}
