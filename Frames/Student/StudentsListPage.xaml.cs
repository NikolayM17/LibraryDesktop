using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Логика взаимодействия для StudentsListPage.xaml
	/// </summary>
	public partial class StudentsListPage : Page
	{
		public StudentsListPage()
		{
			InitializeComponent();

			StudentsDataGrid.ItemsSource = new ObservableCollection<RentRow>();
		}

		private void AllStudentsButton_Click(object sender, RoutedEventArgs e)
		{
			if (AllStudentsButton.Content == "All Students")
			{
				StudentsDataGrid.ItemsSource = new ObservableCollection<Student>();

				AllStudentsButton.Content = "RentList";
			}
			else
			{
				StudentsDataGrid.ItemsSource = new ObservableCollection<RentRow>();

				AllStudentsButton.Content = "All Students";
			}
		}
	}
}
