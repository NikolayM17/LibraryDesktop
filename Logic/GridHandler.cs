using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace LibraryNET6Pages
{
	class GridHandler
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

		public GridHandler(Page pageSender)
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

		public Grid FillGrid(Grid grid, List<Rectangle> rectangles)
		{
			foreach (var rectangle in rectangles)
			{
				grid.Children.Add(rectangle);
			}

			return grid;
		}

		public Border FillBorder(Border border, Book book)
		{
			_currentBook = book;

			border.Child = CreateButton(ImageConverter.Convert.ByteArrayToWpfImage(Convert.FromBase64String(book.Image)));

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

		public Rectangle FillRectangle(Rectangle rectangle, Book book)
		{
			_currentBook = book;

			rectangle.Fill = CreateImageBrush(ImageConverter.Convert.ByteArrayToWpfImage(Convert.FromBase64String(book.Image)));
			rectangle.MouseDown += rectangle_MouseDown;

			SetContextMenu(rectangle, new string[] { "Edit", "Delete" });
			SetToolTip(rectangle, book);

			return rectangle;
		}

		public Rectangle CreateRectangle(int position = 0)
		{
			var rectangle = new Rectangle()
			{
				StrokeThickness = _DefaultThickness.Top,
				Stroke = _DefaultColor,
				RadiusX = 5,
				RadiusY = 5
			};

			SetRectanglePosition(rectangle, position);

			return rectangle;
		}

		private void SetRectanglePosition(Rectangle rectangle, int position = 0)
		{
			Grid.SetColumn(rectangle, _BorderStartCoordinates + (_HorizontalDistance * position));
			Grid.SetColumnSpan(rectangle, _BorderWidth);
			Grid.SetRow(rectangle, _BorderStartCoordinates);
			Grid.SetRowSpan(rectangle, _BorderHeight);
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
					var l = _resourceList.FindAll(i => (i is Style)).Where(i => ((Style)i).TargetType == typeof(MenuItem));

					/*_resourceList.Find(i => (i as Style).TargetType == typeof(MenuItem) *//*new MenuItem().GetType()*//*)*/

					((MenuItem)item).Style = l as Style;
					((MenuItem)item).Click += menuItem_Click;
				}

				var n = _resourceList.FindAll(i => (i is Style)).Where(i => ((Style)i).TargetType == typeof(ContextMenu));

				/*_resourceList.Find(i => (i as Style).TargetType == new ContextMenu().GetType())*/

				contextMenu.Style = n as Style;

				button.ContextMenu = contextMenu;
			}
		}

		private void SetContextMenu(Rectangle rectangle, string[] menuItemsHeaders)
		{
			if (_pageSender is AdminPage)
			{
				var contextMenu = new ContextMenu();

				foreach (string header in menuItemsHeaders)
				{
					var menuItem = new MenuItem() { Header = header };

					contextMenu.Items.Add(menuItem);
				}

				foreach (var item in contextMenu.Items)
				{
					var l = (_resourceList.FindAll(i => (i is Style)))
						.FirstOrDefault(i => ((Style)i).TargetType == typeof(MenuItem));

					((MenuItem)item).Style = l as Style;
					((MenuItem)item).Click += menuItem_Click;
				}

				var n = (_resourceList.FindAll(i => (i is Style)))
					.FirstOrDefault(i => ((Style)i).TargetType == typeof(ContextMenu));

				contextMenu.Style = n as Style;

				rectangle.ContextMenu = contextMenu;
			}
		}

		private void SetToolTip(Rectangle rectangle, Book book)
		{
			var titleLabel = new Label()
			{
				Content = book.Title,
				Foreground = new SolidColorBrush(Colors.White),
				FontFamily = new FontFamily("Century Gothic"),
				FontSize = 15,
				FontWeight = FontWeights.Bold
			};

			var genreLabel = new Label()
			{
				Content = book.Genre,
				Foreground = new SolidColorBrush(Colors.White),
				FontFamily = new FontFamily("Century Gothic"),
				FontSize = 15
			};

			var authorLabel = new Label()
			{
				Content = $"{book.Author} ({book.Date})",
				Foreground = new SolidColorBrush(Colors.White),
				FontFamily = new FontFamily("Century Gothic"),
				FontSize = 15
			};

			var stackPanel = new StackPanel();

			stackPanel.Children.Add(titleLabel);
			stackPanel.Children.Add(genreLabel);
			stackPanel.Children.Add(authorLabel);

			rectangle.ToolTip = new ToolTip()
			{
				Content = stackPanel,
				Background = new SolidColorBrush(Color.FromArgb(100, 144, 0, 255)),
				Foreground = new SolidColorBrush(Colors.White),
				FontFamily = new FontFamily("Century Gothic"),
				FontSize = 15
			};
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

		public ImageBrush CreateImageBrush(Image bookImage)
			=> new ImageBrush()
			{
				Stretch = Stretch.Fill,
				ImageSource = (BitmapImage)bookImage.Source
			};
		
		private async void button_Click(object sender, RoutedEventArgs e)
		{
			if (_pageSender.Opacity == 1)
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
		}

		private void menuItem_Click(object sender, RoutedEventArgs e)
		{
			if (((MenuItem)sender).Header.ToString() == "Delete")
			{
				new MsSqlRepository<AdminPage>().DeleteBook(_currentBook);

				((AdminPage)_pageSender).UpdateSearchResults();
			}
			else if (((MenuItem)sender).Header.ToString() == "Edit")
			{
				_pageSender.NavigationService.Navigate(new EditBookPage(_currentBook));
			}
		}

		private async void rectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (_pageSender.Opacity == 1)
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
			}
		}
	}
}
