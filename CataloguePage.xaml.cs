using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryNET6Pages
{
	/// <summary>
	/// Логика взаимодействия для CataloguePage.xaml
	/// </summary>
	public partial class CataloguePage : Page
	{
		public CataloguePage()
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

		private async void Main_Click(object sender, RoutedEventArgs e)
		{
			EndFrameAnimation();

			await Task.Delay(350);

			NavigationService.Navigate(new MainFrame());
		}

		private void SearchButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			SearchImage.Source = new BitmapImage(new Uri(@"search_pressed.png", UriKind.Relative));
		}

		private void SearchButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			SearchImage.Source = new BitmapImage(new Uri(@"search.png", UriKind.Relative));
		}

		private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
		{

		}

		private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SearchButton_Click_1(object sender, RoutedEventArgs e)
		{

		}
	}
}
