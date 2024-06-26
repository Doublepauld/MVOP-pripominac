﻿using System;
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

            if (opakovani && MainWindow.OpakovaneUdalosti.Count() >= 25)
            {
                MessageBox.Show("Opakovane udalosti jsou plne");

                this.Close();
            }
            if (opakovani == false && MainWindow.NeopakovaneUdalosti.Count() >= 25)
            {
                MessageBox.Show("Neopakovane udalosti jsou plne");

                this.Close();
            }


            if (!int.TryParse(warning_time.Text, out warning))
            {
                
                MessageBox.Show("Warning time must be a valid integer.");
                return;
            }

            Udalost udalost = new Udalost(jmeno, selectedDate ?? DateTime.MinValue, warning, opakovani);


            
            MainWindow.Udalosti.Add(udalost);
            if (opakovani)
            {
                MainWindow.OpakovaneUdalosti.Add(udalost);
                int i = MainWindow.OpakovaneUdalosti.Count;
            }
            else
            {
                MainWindow.NeopakovaneUdalosti.Add(udalost);     
            }




            Jmeno_udalost.Text = "";
            Datum_udalost.Value = null;
            warning_time.Text = "";
            opakovani_udalost.IsChecked = false;

            this.Close();



           
        }


    }
    

   
}

