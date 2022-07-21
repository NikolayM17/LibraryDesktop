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
	/// Логика взаимодействия для BookPage.xaml
	/// </summary>
	public partial class BookPage : Page
	{
		private readonly Page _pageSender;

		public BookPage(Book book, Page pageSender)
		{
			InitializeComponent();

			_pageSender = pageSender;

			NameLabel.Content = book.Title;
			AuthorLabel.Content = book.Author;
			GenreLabel.Content = book.Genre;
			YearLabel.Content = book.Date;
			DescriptionTextBlock.Text = book.Description;
			MaxCountLabel.Content = book.MaxCount;
			BarcodeLabel.Content = book.Barcode;

			RectangleImageBrush.ImageSource = (BitmapImage)ImageConverter.Convert.ByteArrayToWpfImage(
				Convert.FromBase64String(book.Image)
				).Source;

			BarcodeLabel.ToolTip = new ToolTip()
			{
				Content = book.Barcode,
				Background = new SolidColorBrush(Color.FromArgb(75, 144, 0, 255)),
				Foreground = new SolidColorBrush(Colors.White),
				FontFamily = new FontFamily("Century Gothic"),
				FontSize = 15
			};

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

		private async void CatalogueButton_Click(object sender, RoutedEventArgs e)
		{
			if (Opacity == 1)
			{
				EndFrameAnimation();

				await Task.Delay(350);

				NavigationService.Navigate(new CataloguePage());
			}
		}
	}
}
