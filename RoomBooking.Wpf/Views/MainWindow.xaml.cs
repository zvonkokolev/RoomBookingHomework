using RoomBooking.Wpf.ViewModels;

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
            EditCustomerWindow editCustomerWindow = new EditCustomerWindow();
            editCustomerWindow.ShowDialog();
        }
    }
}