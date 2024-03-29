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

namespace MVOP_připomínač
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Open_create(object sender, RoutedEventArgs e)
        {
            Window1 secondWindow = new Window1();

            secondWindow.OpakovaneUdalostiChanged += Window1_OpakovaneUdalostiChanged;
            // Subscribe to the event for NeopakovaneUdalosti
            secondWindow.NeopakovaneUdalostiChanged += Window1_NeopakovaneUdalostiChanged;
            secondWindow.Show();

            List<Udalost> udalosti = new List<Udalost>(secondWindow.Udalosti);
            List<Udalost> opakovaneUdalosti = new List<Udalost>(secondWindow.OpakovaneUdalosti);
            List<Udalost> neopakovaneUdalosti = new List<Udalost>(secondWindow.NeopakovaneUdalosti);

            myListViewOpakovane.DataContext = opakovaneUdalosti;
            myListViewNeopakovane.DataContext = neopakovaneUdalosti;

        }
       

        // Event handler for OpakovaneUdalostiChanged event
        private void Window1_OpakovaneUdalostiChanged(object sender, EventArgs e)
        {
            // Refresh the ListView when OpakovaneUdalosti list changes
            myListViewOpakovane.ItemsSource = (sender as Window1)?.OpakovaneUdalosti;
        }

        // Event handler for NeopakovaneUdalostiChanged event
        private void Window1_NeopakovaneUdalostiChanged(object sender, EventArgs e)
        {
            // Refresh the ListView when NeopakovaneUdalosti list changes
            myListViewNeopakovane.ItemsSource = (sender as Window1)?.NeopakovaneUdalosti;
        }

        


    }


}
