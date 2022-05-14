using Microsoft.Win32;
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
	/// Логика взаимодействия для AddBookPage.xaml
	/// </summary>
	public partial class AddBookPage : Page
	{
		/*private byte[] _bookImage;*/

		public AddBookPage()
		{
			InitializeComponent();
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			var message = new MsSqlController().AddBook(new Book(
				0,
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

	}
}
