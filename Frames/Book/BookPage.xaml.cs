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
	/// Логика взаимодействия для BookPage.xaml
	/// </summary>
	public partial class BookPage : Page
	{
		private readonly Page _pageSender;

		public BookPage() { }

		public BookPage(Book book, Page pageSender)
		{
			InitializeComponent();

			_pageSender = pageSender;

			NameLabel.Content = book.Title;
			AuthorLabel.Content = book.Author;
			GenreLabel.Content = book.Genre;
			YearLabel.Content = book.Date;
			DescriptionTextBlock.Text = book.Description;

			BookImage.Source = ImageConverter.Convert.ByteArrayToWpfImage(
				Convert.FromBase64String(book.Image)
				).Source;
		}

		private void Main_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new MainFrame());
		}

		private void OpenCatalogue(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new CataloguePage());
		}

		private void Genres_Click(object sender, RoutedEventArgs e)
			=> OpenCatalogue(sender, e);

		private void Authors_Click(object sender, RoutedEventArgs e)
			=> OpenCatalogue(sender, e);

		private void Books_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new CataloguePage());
		}
	}
}
