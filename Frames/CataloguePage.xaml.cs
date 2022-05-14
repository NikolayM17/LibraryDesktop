﻿using System;
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
		private MsSqlController _librarydb;

		public CataloguePage()
		{
			InitializeComponent();

			_librarydb = new MsSqlController();

			/*MessageBox.Show(_librarydb.User);*/

			SearchResultsStackPanel.Children.Clear();

			DisplayFoundBooks(_librarydb.AllBooks, this);

			StartFrameAnimation();
		}

		private void DisplayFoundBooks(List<Book> foundBooks, Page page)
		{
			var borderList = new List<Border>();

			var listGridBorders = new List<List<Border>>();

			var gridBooks = new List<Book>();

			var listGridBooks = new List<List<Book>>();

			foreach (var book in foundBooks)
			{
				gridBooks.Add(book);

				if (gridBooks.Count == 4)
				{
					listGridBooks.Add(gridBooks);
					gridBooks = new List<Book>(4);
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

					borderList.Add(gridController
						.FillBorder(gridController
						.CreateBorder(fourBooks.IndexOf(book)), book));
				}

				listGridBorders.Add(borderList);

				borderList = new List<Border>();
			}

			foreach (var fourBorders in listGridBorders)
			{
				var gridController = new GridController(this);

				var grid = gridController.FillGrid(gridController.CreateGrid(), fourBorders);

				grid.UpdateLayout();

				SearchResultsStackPanel.Children.Add(grid);

				SearchResultsStackPanel.UpdateLayout();
			}
		}

		private async void Main_Click(object sender, RoutedEventArgs e)
		{
			EndFrameAnimation();

			await Task.Delay(350);

			NavigationService.Navigate(new MainFrame());
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

		private void StartFrameAnimation()
		{
			/*VerticalAlignment = VerticalAlignment.Center;
			HorizontalAlignment = HorizontalAlignment.Center;*/

			BeginAnimation(WidthProperty, new DoubleAnimation()
			{
				From = 0,
				To = 800,
				Duration = TimeSpan.FromSeconds(0.25)
			});
		}

			/*BeginAnimation(OpacityProperty, new DoubleAnimation()
			{
				From = 0,
				To = 1,
				Duration = TimeSpan.FromSeconds(0.25)
			});*/

		private void EndFrameAnimation() =>
			BeginAnimation(OpacityProperty, new DoubleAnimation()
			{
				From = 1,
				To = 0,
				Duration = TimeSpan.FromSeconds(0.15)
			});
	}
}
