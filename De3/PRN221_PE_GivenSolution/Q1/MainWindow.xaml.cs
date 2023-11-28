using Microsoft.EntityFrameworkCore;
using Q1.Models;
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

namespace Q1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PRN_Spr23_B1Context context;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = context;
            context = new PRN_Spr23_B1Context();
            loadEmployee();
            loadDepartment();
            loadTitleOfCourtesy();
        }
        public void loadEmployee()
        {
            listEmployee.ItemsSource = context.Employees.Include(x => x.Department).ToList(); // dong nay de in ra department.department name 
        }

        public void loadDepartment()
        {
            cbDepartment.ItemsSource = context.Departments.ToList();
            cbDepartment.SelectedItem = 0;
        }
        public void loadTitleOfCourtesy()
        {
            var uniqueTitleOfCourtesy = context.Employees
                .Select(e => e.TitleOfCourtesy)
                .Distinct()
                .ToList();

            cbTitleOfCourtesy.ItemsSource = uniqueTitleOfCourtesy;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbEmployeeId.Text = string.Empty;
            tbFirstName.Text = string.Empty;
            tbLastName.Text = string.Empty;
            cbDepartment.Text = string.Empty;
            tbTitle.Text = string.Empty;
            cbTitleOfCourtesy.Text = string.Empty;
            dtBirthdate.SelectedDate = null;
        }

        private void listEmployee_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Employee item = (sender as ListView).SelectedItem as Employee;
            if (item != null)
            {
                tbEmployeeId.Text = item.EmployeeId.ToString();
                tbFirstName.Text = item.FirstName;
                tbLastName.Text = item.LastName;
                cbDepartment.SelectedItem = item.Department;
                tbTitle.Text = item.Title;
                cbTitleOfCourtesy.SelectedItem = item.TitleOfCourtesy;
                dtBirthdate.SelectedDate = item.BirthDate;
            }
        }
    }
}
