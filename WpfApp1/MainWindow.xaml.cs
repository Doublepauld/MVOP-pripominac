using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Newtonsoft.Json;
using System.IO;

namespace MVOP_připomínač
{
   
   

    public partial class MainWindow : Window
    {
        public static List<Udalost> Udalosti = new List<Udalost>();
        public static ObservableCollection<Udalost> OpakovaneUdalosti { get; set; }
        public static ObservableCollection<Udalost> NeopakovaneUdalosti { get; set; }


        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1); // Set the interval to 1 minute
            timer.Tick += Timer_Tick; // Add an event handler for the Tick event
            timer.Start(); // Start the timer

            string opakovaneFilePath = "opakovane.json";
            string neopakovaneFilePath = "neopakovane.json";

            if (!File.Exists(opakovaneFilePath))
            {
                // vytvořit file pokud neexistuje
                File.WriteAllText(opakovaneFilePath, "[]");
            }

            if (!File.Exists(neopakovaneFilePath))
            {
                // vytvořit file pokud neexistuje
                File.WriteAllText(neopakovaneFilePath, "[]");
            }

            // načtení dat z json
            string opakovaneJson = File.ReadAllText(opakovaneFilePath);
            string neopakovaneJson = File.ReadAllText(neopakovaneFilePath);

            // Deserialize json do kolekce
            OpakovaneUdalosti = JsonConvert.DeserializeObject<ObservableCollection<Udalost>>(opakovaneJson) ?? new ObservableCollection<Udalost>();
            NeopakovaneUdalosti = JsonConvert.DeserializeObject<ObservableCollection<Udalost>>(neopakovaneJson) ?? new ObservableCollection<Udalost>();

            
            myListViewOpakovane.ItemsSource = OpakovaneUdalosti;
            myListViewNeopakovane.ItemsSource = NeopakovaneUdalosti;

            Application.Current.Exit += AppExit;
        }

      

        
        private void AppExit(object sender, ExitEventArgs e)
        {
            // Save OpakovaneUdalosti to opakovane.json
            string opakovaneJson = JsonConvert.SerializeObject(OpakovaneUdalosti);
            File.WriteAllText("opakovane.json", opakovaneJson);

            // Save NeopakovaneUdalosti to neopakovane.json
            string neopakovaneJson = JsonConvert.SerializeObject(NeopakovaneUdalosti);
            File.WriteAllText("neopakovane.json", neopakovaneJson);
        }

        // check warning and time of Event every minute
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;

            // Mark events for removal
            List<Udalost> itemsToRemove = new List<Udalost>();
            List<Udalost> itemsToAdd = new List<Udalost>();
            List<Udalost> itemsToRemoveNe = new List<Udalost>();

            //warning opakovat
            foreach (Udalost udalost in OpakovaneUdalosti)
            {
                if (udalost.Warning.Hour == currentTime.Hour && udalost.Warning.Minute == currentTime.Minute)
                {
                    MessageBox.Show($"Připomínka události {udalost.Jmeno}.");
                }
            }

            // warning neopakovat
            foreach (Udalost udalost in NeopakovaneUdalosti)
            {
                if (udalost.Warning.Hour == currentTime.Hour && udalost.Warning.Minute == currentTime.Minute)
                {
                    MessageBox.Show($"Připomínka události {udalost.Jmeno}.");
                }
            }

            // time opakovat
            for (int i = OpakovaneUdalosti.Count - 1; i >= 0; i--)
            {
                Udalost udalost = OpakovaneUdalosti[i];

                if (udalost.Date.Hour == currentTime.Hour && udalost.Date.Minute == currentTime.Minute)
                {
                    MessageBox.Show($"Začla událost {udalost.Jmeno}.");
                    itemsToRemove.Add(udalost);
                    // Increment the date by 1 week
                    udalost.Date = udalost.Date.AddDays(7);
                    udalost.Warning = udalost.Warning.AddDays(7);

                    // Mark the event for removal
                    

                    itemsToAdd.Add(udalost);    

                }
            }


            // Remove marked items
            foreach (Udalost udalostToRemove in itemsToRemove)
            {
                OpakovaneUdalosti.Remove(udalostToRemove);
            }

            foreach (var item in itemsToAdd)
            {
                OpakovaneUdalosti.Add(item);

            }

            // time neopakovat
            foreach (Udalost udalost in NeopakovaneUdalosti)
            {
                if (udalost.Date.Hour == currentTime.Hour && udalost.Date.Minute == currentTime.Minute)
                {
                    MessageBox.Show($"Začla událost {udalost.Jmeno}.");

                    itemsToRemoveNe.Add(udalost);
                }
            }

            // Remove marked items
            foreach (Udalost udalostToRemove in itemsToRemoveNe)
            {
                NeopakovaneUdalosti.Remove(udalostToRemove);
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            Udalost udalost = button.DataContext as Udalost;

            Window3 window3 = new Window3(udalost);
            window3.Show();

        }

        // Event handler for the delete button
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            Udalost udalost = button.DataContext as Udalost;

            Window2 deleteWindow = new Window2(udalost);

            deleteWindow.Show();
            
           
        }

        private void Open_create(object sender, RoutedEventArgs e)
        {
            if (OpakovaneUdalosti.Count >= 25 && NeopakovaneUdalosti.Count >= 25)
            {
                MessageBox.Show("Máte plný počet událostí v obou listech");
            }
            else
            {
                Window1 secondWindow = new Window1();
                secondWindow.Show();
            }
           
        }
        

    }
    [Serializable]
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
