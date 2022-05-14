using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	/// Логика взаимодействия для EditBookPage.xaml
	/// </summary>
	public partial class EditBookPage : Page
	{
		private int _id;

		public EditBookPage(Book book)
		{
			InitializeComponent();

			_id = book.Id;

			TitleTextBox.Text = book.Title;
			AuthorTextBox.Text = book.Author;
			GenreTextBox.Text = book.Genre;
			/*GenreCb.SelectedItem = book.Genre;*/
			YearTextBox.Text = book.Date.ToString();
			DescriptionTextBox.Text = book.Description;

			BookImage.Source = ImageConverter.Convert.ByteArrayToWpfImage(
				Convert.FromBase64String(book.Image)
				).Source;

			TitleWaterMark.Visibility = TitleTextBox.Text.Length == 0 ? Visibility.Visible : Visibility.Hidden;
			DescriptionWatermark.Visibility = DescriptionTextBox.Text.Length == 0 ? Visibility.Visible : Visibility.Hidden;

			/*GenreCb.ItemsSource = new MsSqlController().GenreList;*/
				/*new string[] { "Genre1", "Genre2", "Genre7" };*/
		}

		private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();

			fileDialog.Title = "Select a picture";
			fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
			  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
			  "Portable Network Graphic (*.png)|*.png";

			if (fileDialog.ShowDialog() == true)
			{
				/*_bookImage = File.ReadAllBytes(fileDialog.FileName);*/

				BookImage.Source = new BitmapImage(new Uri(fileDialog.FileName));
				BookImage.Stretch = System.Windows.Media.Stretch.Fill;
			}
		}

		private void TitleTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			TitleWaterMark.Visibility = Visibility.Collapsed;
		}

		private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (TitleTextBox.Text.Length == 0)
			{
				TitleWaterMark.Visibility = Visibility.Visible;
			}
		}

		private void DescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			DescriptionWatermark.Visibility = Visibility.Collapsed;
		}

		private void DescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (DescriptionTextBox.Text.Length == 0)
			{
				DescriptionWatermark.Visibility = Visibility.Visible;
			}
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			var message = new MsSqlController().EditBook(new Book(
				_id,
				TitleTextBox.Text,
				AuthorTextBox.Text,
				GenreTextBox.Text,
				DescriptionTextBox.Text,
				ImageConverter.Convert.WpfImageToByteArray(BookImage),
				short.Parse(YearTextBox.Text)));

			if (!(message is null))
			{
				MessageBox.Show(message);
			}

			else
			{
				MessageBox.Show("Done");
			}
		}

		private void OpenPdfButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo(@"D:\Downloads\85521056.a4\85521056.a4.pdf") { UseShellExecute = true });
		}

		private void GenreCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			/*GenreTextBox.Text = ((ComboBox)sender).SelectedItem as string;*/
		}

		private void OpenStudentsList_Click(object sender, RoutedEventArgs e)
		{
			/*NavigationService.Navigate(new StudentsListPage());*/

			var childWindow = new StudentsListWindow(_id);

			childWindow.ShowDialog();
		}
	}
}
