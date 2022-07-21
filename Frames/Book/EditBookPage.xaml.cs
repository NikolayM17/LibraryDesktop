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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LibraryNET6Pages.Controllers;

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
			YearTextBox.Text = book.Date.ToString() == "-1" ? "" : book.Date.ToString();
			DescriptionTextBox.Text = book.Description;
			MaxCountTextBox.Text = book.MaxCount.ToString() == "-1" ? "" : book.MaxCount.ToString();
			BarcodeTextBox.Text = book.Barcode.ToString() == "-1" ? "" : book.Barcode.ToString();

			/*BookImage.Source = ImageController.Convert.ByteArrayToWpfImage(
				Convert.FromBase64String(book.Image)
				).Source;*/

			openFdRectangleImageBrush.ImageSource = (BitmapImage)ImageConverter.Convert.ByteArrayToWpfImage(
				Convert.FromBase64String(book.Image)
				).Source;

			TitleWaterMark.Visibility = TitleTextBox.Text.Length == 0 ?
				Visibility.Visible : Visibility.Hidden;
			DescriptionWatermark.Visibility = DescriptionTextBox.Text.Length == 0 ?
				Visibility.Visible : Visibility.Hidden;

			BarcodeWaterMark.Visibility = BarcodeTextBox.Text.Length == 0 ?
				Visibility.Visible : Visibility.Hidden;

			StartFrameAnimation();

			/*GenreCb.ItemsSource = new MsSqlController().GenreList;*/
				/*new string[] { "Genre1", "Genre2", "Genre7" };*/
		}

		private void StartFrameAnimation() =>
			BeginAnimation(OpacityProperty, new DoubleAnimation()
			{
				From = 0,
				To = 1,
				Duration = TimeSpan.FromSeconds(0.15)
			});

		private void EndFrameAnimation() =>
			BeginAnimation(OpacityProperty, new DoubleAnimation()
			{
				From = 1,
				To = 0,
				Duration = TimeSpan.FromSeconds(0.15)
			});

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
			if (IsBookFilled() && IsBookCorrect())
			{
				var message = new MsSqlRepository<AdminPage>().EditBook(new Book(
					_id,
					TitleTextBox.Text,
					AuthorTextBox.Text,
					GenreTextBox.Text,
					DescriptionTextBox.Text,
					/*ImageController.Convert.WpfImageToByteArray(BookImage),*/
					ImageConverter.Convert.WpfImageToByteArray(new Image()
					{
						Source = (BitmapImage)openFdRectangleImageBrush.ImageSource
					}),
					int.TryParse(YearTextBox.Text, out int year) ? year : -1,
					int.TryParse(MaxCountTextBox.Text, out int count) ? count : -1,
					long.TryParse(BarcodeTextBox.Text, out long barcode) ? barcode : -1
					));

				MessageBox.Show(message is not null ? message : "Done");
			}
		}

		private bool IsBookFilled()
		{
			if (TitleTextBox.Text.Length == 0)
			{
				MessageBox.Show("Введите название книги");
				return false;
			}
			else
			{
				if (AuthorTextBox.Text.Length == 0 &&
					MessageBox.Show("Введите автора книги", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					return false;
				}
				if (GenreTextBox.Text.Length == 0 &&
					MessageBox.Show("Введите жанр книги", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					return false;
				}
				if (YearTextBox.Text.Length == 0 &&
					MessageBox.Show("Введите дату выпуска книги", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					return false;
				}
				if (MaxCountTextBox.Text.Length == 0 &&
					MessageBox.Show("Введите количество оставшихся книг", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					return false;
				}
				if (BarcodeTextBox.Text.Length == 0 &&
					MessageBox.Show("Введите код книги", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					return false;
				}
			}

			return true;
		}

		private bool IsBookCorrect()
		{
			bool result = false;

			if (!InputDataHandler.IsDataParsedToInt(YearTextBox.Text))
			{
				MessageBox.Show("Проверьте год выпуска книги");
			}
			else if (!InputDataHandler.IsDataParsedToInt(MaxCountTextBox.Text))
			{
				MessageBox.Show("Проверьте количество оставшихся книг");
			}
			else if (!InputDataHandler.IsDataParsedToLong(BarcodeTextBox.Text))
			{
				MessageBox.Show("Проверьте код книги");
			}
			else result = true;

			return result;
		}

		private void OpenPdfButton_Click(object sender, RoutedEventArgs e)
		{
			//	Process.Start(new ProcessStartInfo(@"D:\Downloads\85521056.a4\85521056.a4.pdf") { UseShellExecute = true });

			MessageBox.Show("Soon");
		}

		private void GenreCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			/*GenreTextBox.Text = ((ComboBox)sender).SelectedItem as string;*/
		}

		private void OpenStudentsList_Click(object sender, RoutedEventArgs e)
		{
			/*NavigationService.Navigate(new StudentsListPage());*/

			var childWindow = new StudentsListWindow(_id) {Title = TitleTextBox.Text };

			if(childWindow.ShowDialog() == false)
			{
				UpdateCount();
			}
		}

		private void UpdateCount()
		{
			if (InputDataHandler.IsDataParsedToInt(MaxCountTextBox.Text))
			{
				int count = MsSqlRepository<AdminPage>.GetRemainingCount(_id);

				MaxCountTextBox.Text = count >= 0 ? count.ToString() : "";
			}
		}

		private async void AdminButton_Click(object sender, RoutedEventArgs e)
		{
			if (Opacity == 1)
			{
				EndFrameAnimation();

				await Task.Delay(350);

				NavigationService.Navigate(new AdminPage());
			}
		}

		private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				OpenFileDialog fileDialog = new OpenFileDialog();

				fileDialog.Title = "Select a picture";
				fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
				  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
				  "Portable Network Graphic (*.png)|*.png";

				if (fileDialog.ShowDialog() == true)
				{
					openFdRectangleImageBrush.ImageSource = new BitmapImage(new Uri(fileDialog.FileName));
					openFdRectangleImageBrush.Stretch = System.Windows.Media.Stretch.Fill;

					var img = new Image()
					{
						Source = (BitmapImage)openFdRectangleImageBrush.ImageSource
					};
				}
			}
		}

		private void BarcodeTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			BarcodeWaterMark.Visibility = Visibility.Hidden;
		}

		private void BarcodeTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (BarcodeTextBox.Text.Length == 0)
			{
				BarcodeWaterMark.Visibility = Visibility.Visible;
			}
		}
	}
}
