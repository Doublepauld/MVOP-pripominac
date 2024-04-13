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
    /// Interakční logika pro Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {

        private Udalost udalost;
        public Window3()
        {
            InitializeComponent();
        }

        public Window3(Udalost udalost) : this()
        {
            // Store the passed Udalost object
            this.udalost = udalost;

            // Fill the fields with values from the Udalost object
            Jmeno_udalost.Text = udalost.Jmeno;
            Datum_udalost.Value = udalost.Date;
            warning_time.Text = (udalost.Warning.Minute).ToString();
            opakovani_udalost.IsChecked = udalost.Opakovani;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (!IsPositiveNumber(e.Text) || (sender as TextBox).Text.Length >= 3)
            {
                e.Handled = true;
            }
        }

        bool IsPositiveNumber(string text)
        {

            if (double.TryParse(text, out double number))
            {
                return number >= 0;
            }
            return false;
        }

        private void CreateUdalostButton_Click(object sender, RoutedEventArgs e)
        {
            if (udalost != null)
            {
                if (udalost.Opakovani)
                {
                    MainWindow.OpakovaneUdalosti.Remove(udalost);
                }
                else
                {
                    MainWindow.NeopakovaneUdalosti.Remove(udalost);
                }
            }

            string jmeno = Jmeno_udalost.Text;
            DateTime? selectedDate = Datum_udalost.Value;
            int warning;
            bool opakovani = opakovani_udalost.IsChecked ?? false;

            if (!int.TryParse(warning_time.Text, out warning))
            {
                MessageBox.Show("Warning time must be a valid integer.");
                return;
            }

            Udalost newUdalost = new Udalost(jmeno, selectedDate ?? DateTime.MinValue, warning, opakovani);

            MainWindow.Udalosti.Add(newUdalost);
            if (opakovani)
            {
                MainWindow.OpakovaneUdalosti.Add(newUdalost);
            }
            else
            {
                MainWindow.NeopakovaneUdalosti.Add(newUdalost);
            }

            Jmeno_udalost.Text = "";
            Datum_udalost.Value = null;
            warning_time.Text = "";
            opakovani_udalost.IsChecked = false;

            this.Close();
        }

      
    }
}
