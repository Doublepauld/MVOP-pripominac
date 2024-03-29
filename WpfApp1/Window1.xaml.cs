using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interakční logika pro Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public List<Udalost> Udalosti { get; } = new List<Udalost>();
        public List<Udalost> OpakovaneUdalosti { get; } = new List<Udalost>();
        public List<Udalost> NeopakovaneUdalosti { get; } = new List<Udalost>();

        public event EventHandler OpakovaneUdalostiChanged;
        public event EventHandler NeopakovaneUdalostiChanged;



        public Window1()
        {
            InitializeComponent();
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

            // Add the new Udalost to the existing lists
            Udalosti.Add(newUdalost);

            if (opakovani)
            {
                OpakovaneUdalosti.Add(newUdalost);
                OnOpakovaneUdalostiChanged();
            }
            else
            {
                NeopakovaneUdalosti.Add(newUdalost);
                OnNeopakovaneUdalostiChanged();
            }




            Jmeno_udalost.Text = "";
            Datum_udalost.Value = null;
            warning_time.Text = "";
            opakovani_udalost.IsChecked = false;

            this.Close();



           
        }

        protected virtual void OnOpakovaneUdalostiChanged()
        {
            OpakovaneUdalostiChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnNeopakovaneUdalostiChanged()
        {
            NeopakovaneUdalostiChanged?.Invoke(this, EventArgs.Empty);
        }


    }
    public class Udalost
    {
        string jmeno;
        public string Jmeno
        {
            get { return jmeno; }
            set { jmeno = value; }
        }

        DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        DateTime warning;
        public DateTime Warning
        {
            get { return warning; }
            set { warning = value; }
        }

        bool opakovani;
        public bool Opakovani
        {
            get { return opakovani; }
            set { opakovani = value; }
        }

        public Udalost(string jmeno, DateTime date, int? warningMinutes = null, bool opakovani = false)
        {
            this.Jmeno = jmeno;
            this.Date = date;
            this.Warning = date.AddMinutes(-(warningMinutes ?? 30));
            this.Opakovani = opakovani;
        }
    }

   
}

