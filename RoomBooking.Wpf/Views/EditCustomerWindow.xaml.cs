using RoomBooking.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RoomBooking.Wpf.Views
{
    /// <summary>
    /// Interaktionslogik für EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow
    {
        public EditCustomerWindow(Core.Entities.Customer sourceCustomer)
        {
            var ctrl = this.DataContext;
            CustomerViewModel customerViewModel = new CustomerViewModel(null, sourceCustomer);
            InitializeComponent();
        }
    }
}
