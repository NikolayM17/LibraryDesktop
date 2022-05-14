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
	/// Логика взаимодействия для AdminEnterPage.xaml
	/// </summary>
	public partial class AdminEnterPage : Page
	{
		public AdminEnterPage()
		{
			InitializeComponent();
		}

		private void TextBox_MouseEnter(object sender, MouseEventArgs e)
		{
			if (!((TextBox)sender).IsFocused)
			{
				((TextBox)sender).Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
			}
		}

		private void TextBox_MouseLeave(object sender, MouseEventArgs e)
		{
			if (!((TextBox)sender).IsFocused)
			{
				((TextBox)sender).Background = new SolidColorBrush(Colors.White);
			}
		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			LoginWatermark.Visibility = Visibility.Collapsed;

			((TextBox)sender).Background = new SolidColorBrush(Colors.White);

			((TextBox)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(200, 100, 255));

			((TextBox)sender).Foreground = new SolidColorBrush(Color.FromRgb(144, 0, 255));
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (((TextBox)sender).Text.Length == 0)
			{
				LoginWatermark.Visibility = Visibility.Visible;
			}
			((TextBox)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(144, 0, 255));
		}

		private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			PasswordWatermark.Visibility = Visibility.Collapsed;

			((PasswordBox)sender).Background = new SolidColorBrush(Colors.White);

			((PasswordBox)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(200, 100, 255));

			((PasswordBox)sender).Foreground = new SolidColorBrush(Color.FromRgb(144, 0, 255));
		}

		private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (((PasswordBox)sender).Password.Length == 0)
			{
				PasswordWatermark.Visibility = Visibility.Visible;
			}

			((PasswordBox)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(144, 0, 255));
		}

		private void PasswordTextBox_MouseEnter(object sender, MouseEventArgs e)
		{
			if (!((PasswordBox)sender).IsFocused)
			{
				((PasswordBox)sender).Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
			}
		}

		private void PasswordTextBox_MouseLeave(object sender, MouseEventArgs e)
		{
			if (!((PasswordBox)sender).IsFocused)
			{
				((PasswordBox)sender).Background = new SolidColorBrush(Colors.White);
			}
		}

		private async void AdminEnterButton_Click(object sender, RoutedEventArgs e)
		{
			var isEnterDataCorrect = MsSqlController.IsEnterDataCorrect(LoginTextBox.Text, PasswordTextBox.Password);

			if (isEnterDataCorrect.Item1 && isEnterDataCorrect.Item2)
			{
				await Task.Delay(250);

				NavigationService.Navigate(new AdminPage());
			}
			else if (isEnterDataCorrect.Item1)
			{
				MessageBox.Show("Irregular Password");
			}
			else if (isEnterDataCorrect.Item2)
			{
				MessageBox.Show("Irregular Login");
			}
			else
			{
				MessageBox.Show("Irregular Login or Password");
			}
		}

		private void Page_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				AdminEnterButton_Click(sender, e);
			}
		}
	}
}
