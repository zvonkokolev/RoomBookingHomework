using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Persistence;
using RoomBooking.Wpf.ViewModels;
using System.Windows.Controls;

namespace RoomBooking.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClick_EditCustomerWindow(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            Customer sourceCustomer = (Customer)button.CommandParameter;

            EditCustomerWindow editCustomerWindow = new EditCustomerWindow(sourceCustomer);
            editCustomerWindow.ShowDialog();
        }

    }
}