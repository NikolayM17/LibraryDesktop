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
using System.Windows.Media.Animation;
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

			SearchResultsStackPanel.Children.Clear();

			DisplayFoundBooks(_librarydb.AllBooks, this);

			StartFrameAnimation();
		}

		private void StartFrameAnimation() =>
			BeginAnimation(OpacityProperty, new DoubleAnimation()
			{
				From = 0,
				To = 1,
				Duration = TimeSpan.FromSeconds(0.15)
			});

		public void EndFrameAnimation() =>
			BeginAnimation(OpacityProperty, new DoubleAnimation()
			{
				From = 1,
				To = 0,
				Duration = TimeSpan.FromSeconds(0.15)
			});

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

		private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			SearchWatermark.Visibility = Visibility.Collapsed;

			((TextBox)sender).Background = new SolidColorBrush(Colors.White);

			((TextBox)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(200, 100, 255));

			((TextBox)sender).Foreground = new SolidColorBrush(Color.FromRgb(144, 0, 255));
		}

		private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (((TextBox)sender).Text.Length == 0)
			{
				SearchWatermark.Visibility = Visibility.Visible;
			}
			((TextBox)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(144, 0, 255));
		}

		private void SearchTextBox_MouseEnter(object sender, MouseEventArgs e)
		{
			if (!((TextBox)sender).IsFocused)
			{
				((TextBox)sender).Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
			}
		}

		private void SearchTextBox_MouseLeave(object sender, MouseEventArgs e)
		{
			if (!((TextBox)sender).IsFocused)
			{
				((TextBox)sender).Background = new SolidColorBrush(Colors.White);
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
		}

		private void AddBookButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			AddBookImage.Source = new BitmapImage(new Uri(@"/assets/add_icon.png", UriKind.Relative));
		}

		private async void AddBookButton_Click(object sender, RoutedEventArgs e)
		{
			if (Opacity == 1)
			{
				EndFrameAnimation();

				await Task.Delay(350);

				NavigationService.Navigate(new AddBookPage());
			}
		}

		private async void MainButton_Click(object sender, RoutedEventArgs e)
		{
			if (Opacity == 1)
			{
				EndFrameAnimation();

				await Task.Delay(350);

				NavigationService.Navigate(new MainFrame());
			}
		}
	}
}
