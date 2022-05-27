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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Show();
			
			this.MainFrame.Content = new MainFrame();

			/*var heightAnimation = new DoubleAnimation()
			{
				From = grid.ActualHeight,
				To = 450,
				Duration = TimeSpan.FromMilliseconds(300)
			};

			grid.BeginAnimation(HeightProperty, heightAnimation);*/

			/*IconImage.Source = new BitmapImage(new Uri(@"dIcon.png", UriKind.Relative));*/
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left/* && IsFocused*/)
				this.DragMove();
		}
	}
}
