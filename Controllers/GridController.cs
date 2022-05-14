using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading.Tasks;

namespace LibraryNET6Pages
{
	class GridController
	{
		private readonly Page _pageSender;

		private const int _HorizontalDistance = 5;

		private const int _RowCount = 15;
		private const int _ColumnCount = 20;

		private const int _MinHeight = 200;

		private const int _BorderStartCoordinates = 1;

		private const int _BorderWidth = 3;
		private const int _BorderHeight = 13;

		private readonly Thickness _DefaultThickness = new Thickness(0, 1, 0, 1);
		private readonly SolidColorBrush _DefaultColor = new SolidColorBrush(Color.FromRgb(0, 0, 255));

		private Book _currentBook;

		private List<object> _resourceList;

		public GridController(Page pageSender)
		{
			_pageSender = pageSender;

			_resourceList = new List<object>();

			foreach (object elem in pageSender.Resources.Values)
			{
				_resourceList.Add(elem);
			}
		}

		public Grid CreateGrid()
		{
			var grid = new Grid();

			grid.MinHeight = _MinHeight;

			for (int i = 0; i < _ColumnCount; i++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition());
			}

			for (int i = 0; i < _RowCount; i++)
			{
				grid.RowDefinitions.Add(new RowDefinition());
			}

			return grid;
		}

		public Grid FillGrid(Grid grid, List<Border> borders)
		{
			foreach (var border in borders)
			{
				grid.Children.Add(border);
			}

			return grid;
		}

		public Border FillBorder(Border border, Book book)
		{
			_currentBook = book;

			border.Child = CreateButton(ImageController.Convert.ByteArrayToWpfImage(Convert.FromBase64String(book.Image)));

			return border;
		}

		public Border CreateBorder(int position = 0)
		{
			var border = new Border()
			{
				BorderThickness = _DefaultThickness,
				BorderBrush = _DefaultColor
			};

			SetBorderPosition(border, position);

			return border;
		}

		private void SetBorderPosition(Border border, int position = 0)
		{
			Grid.SetColumn(border, _BorderStartCoordinates + (_HorizontalDistance * position));
			Grid.SetColumnSpan(border, _BorderWidth);
			Grid.SetRow(border, _BorderStartCoordinates);
			Grid.SetRowSpan(border, _BorderHeight);
		}

		public Button CreateButton(Image bookImage)
		{
			var button = new Button();

			button.Content = CreateImage(bookImage);
			button.Click += button_Click;

			button.Style = (Style)_pageSender.FindResource("ImageButton");

			SetContextMenu(button, new string[] { "Edit", "Delete" });

			return button;
		}

		private void SetContextMenu(Button button, string[] menuItemsHeaders)
		{
			if (_pageSender is AdminPage)
			{
				var contextMenu = new ContextMenu();

				foreach(string header in menuItemsHeaders)
				{
					var menuItem = new MenuItem() { Header = header };

					contextMenu.Items.Add(menuItem);
				}

				/*var item1 = new MenuItem() { Header = "Edit" };
				var item2 = new MenuItem() { Header = "Delete" };

				contextMenu.Items.Add(item1);
				contextMenu.Items.Add(item2);*/

				foreach (var item in contextMenu.Items)
				{
					((MenuItem)item).Style = _resourceList.Find(i => ((Style)i).TargetType == new MenuItem().GetType()) as Style;
					((MenuItem)item).Click += menuItem_Click;
				}

				contextMenu.Style = _resourceList.Find(i => ((Style)i).TargetType == new ContextMenu().GetType()) as Style;

				button.ContextMenu = contextMenu;
			}
		}

		public Image CreateImage(Image bookImage)
			=> new System.Windows.Controls.Image()
			{
				Stretch = Stretch.Fill,
				Source = bookImage.Source

				/*ImageConverter.Convert.ByteArrayToWpfImage(
					ImageConverter.Convert.ImageToByteArray(bookImage)
					).Source*/
			};

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			if (_pageSender is AdminPage)
			{
				((AdminPage)_pageSender).EndFrameAnimation();
				
				await Task.Delay(350);

				_pageSender.NavigationService.Navigate(new EditBookPage(_currentBook));
			}
			else
			{
				((CataloguePage)_pageSender).EndFrameAnimation();

				await Task.Delay(350);

				_pageSender.NavigationService.Navigate(new BookPage(_currentBook, _pageSender));
			}
		}

		private void menuItem_Click(object sender, RoutedEventArgs e)
		{
			if (((MenuItem)sender).Header.ToString() == "Delete")
			{
				new MsSqlController().DeleteBook(_currentBook);

				/*((MenuItem)sender).Header = "Deleted";*/
			}
			else if (((MenuItem)sender).Header.ToString() == "Edit")
			{
				_pageSender.NavigationService.Navigate(new EditBookPage(_currentBook));

				/*((MenuItem)sender).Header = "Edited";*/
			}
		}
	}
}
