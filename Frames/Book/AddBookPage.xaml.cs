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
using System.Windows.Media.Animation;
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

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			if (IsBookFilled())
			{
				var message = new MsSqlController<AdminPage>().AddBook(new Book(
					0,
					TitleTextBox.Text,
					AuthorTextBox.Text,
					GenreTextBox.Text,
					DescriptionTextBox.Text,
					/*ImageController.Convert.WpfImageToByteArray(BookImage),*/
					ImageController.Convert.WpfImageToByteArray(new Image()
					{
						Source = openFdRectangleImageBrush.ImageSource
					}),
					short.Parse(YearTextBox.Text.Length == 0 ? "-1" : YearTextBox.Text),
					int.Parse(MaxCountTextBox.Text.Length == 0 ? "-1" : MaxCountTextBox.Text),
					long.Parse(BarcodeTextBox.Text.Length == 0 ? "-1" : BarcodeTextBox.Text)));

				MessageBox.Show(message is not null ? message : "Done");
			}
		}

		private bool IsBookFilled()
		{
			bool titleIsReady = false;
			bool authorIsReady = false;
			bool genreIsReady = false;
			bool dateIsReady = false;
			bool descriptionIsReady = false;
			bool countIsReady = false;

			if (TitleTextBox.Text.Length == 0)
			{
				MessageBox.Show("Введите название книги");
			}
			else
			{
				titleIsReady = true;

				if (AuthorTextBox.Text.Length == 0)
				{
					if (MessageBox.Show("Введите автора книги", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
					{
						authorIsReady = true;

						if (GenreTextBox.Text.Length == 0)
						{
							if (MessageBox.Show("Введите жанр книги", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
							{
								genreIsReady = true;

								if (YearTextBox.Text.Length == 0)
								{
									if (MessageBox.Show("Введите дату выпуска книги", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
									{
										dateIsReady = true;

										if (DescriptionTextBox.Text.Length == 0)
										{
											if (MessageBox.Show("Введите описание книги", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
											{
												descriptionIsReady = true;

												if (MaxCountTextBox.Text.Length == 0)
												{
													if (MessageBox.Show("Введите количество оставшихся экземпляров", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
													{
														countIsReady = true;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			
			return titleIsReady && authorIsReady && genreIsReady && dateIsReady && descriptionIsReady && countIsReady;
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
