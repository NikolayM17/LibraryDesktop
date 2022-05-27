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

using LibraryNET6Pages.Controllers;

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

			StartFrameAnimation();
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
			var isEnterDataCorrect = InputDataController.CheckAdmin(LoginTextBox.Text, PasswordTextBox.Password);

			if (isEnterDataCorrect.Item1 && isEnterDataCorrect.Item2)
			{
				if (Opacity == 1)
				{
					EndFrameAnimation();

					await Task.Delay(150);

					NavigationService.Navigate(new AdminPage());
				}
			}
			else if (isEnterDataCorrect.Item1)
			{
				MessageBox.Show("Irregular Password");
			}
			else if (isEnterDataCorrect.Item2)
			{
				MessageBox.Show("Irregular Login");
			}
		}

		private void Page_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				AdminEnterButton_Click(sender, e);
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
