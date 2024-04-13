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
using System.Windows.Shapes;

namespace MVOP_připomínač
{
    /// <summary>
    /// Interakční logika pro Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private Udalost udalost;
        public Window2(Udalost udalost)
        {
               InitializeComponent();
            this.udalost = udalost;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

            if (udalost.Opakovani)
            {
                MainWindow.OpakovaneUdalosti.Remove(udalost);
            }
            else
            {
                MainWindow.NeopakovaneUdalosti.Remove(udalost);
            }
            this.Close();
        }

        // Event handler for the "refuse" button click event
        private void Refuse_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
