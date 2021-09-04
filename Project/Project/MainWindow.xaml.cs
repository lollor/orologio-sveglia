using System;
using System.Collections.Generic;
using System.IO;
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

namespace Project
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string PATH_SALVATAGGIO_DATI_CITTA = "dati-citta.txt";
        ListaCitta listaCitta;
        List<Grid> listaGrid;
        public MainWindow()
        {
            listaGrid = new List<Grid>();
            listaCitta = new ListaCitta();
            InitializeComponent();
            LogPersonalizzato.Log("Programma aperto.");
            //Meteo meteo = new Meteo("Trezzano Rosa");
        }
        Grid GetDefaultGrid(Citta citta)
        {
            LogPersonalizzato.Log("Inizio a creare grid.");
            Grid grid = new Grid();
            //Citta Citta = new Citta(citta);
            //Meteo meteo = new Meteo(citta);
            //grid settings
            grid.Height = 73;
            grid.Width = 267;
            grid.Background = Brushes.LightGray;
            grid.Margin = new Thickness(10, 38+listaGrid.Count*79, 0, 0);
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;

            //create labelCitta
            Label labelCitta = new Label();
            labelCitta.HorizontalAlignment = HorizontalAlignment.Left;
            labelCitta.Height = 31;
            labelCitta.Margin = new Thickness(3, 5, 0, 0);
            labelCitta.VerticalAlignment = VerticalAlignment.Top;
            labelCitta.Width = 153;
            labelCitta.VerticalContentAlignment = VerticalAlignment.Center;
            //while (!Citta.IsInfoReceived() && !Citta.IsInfoCorrect()) ;
            labelCitta.FontWeight = FontWeights.Bold;

            //create image
            Image image = new Image();
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.Height = 31;
            image.Margin = new Thickness(167, 21, 0, 0);
            image.VerticalAlignment = VerticalAlignment.Top;
            image.Width = 31;
            

            //create labelTemp
            Label labelTemp = new Label();
            labelTemp.HorizontalAlignment = HorizontalAlignment.Left;
            labelTemp.Height = 31;
            labelTemp.Margin = new Thickness(207, 21, 0, 0);
            labelTemp.VerticalAlignment = VerticalAlignment.Top;
            labelTemp.Width = 53;
            labelTemp.VerticalContentAlignment = VerticalAlignment.Center;
            
            labelTemp.HorizontalContentAlignment = HorizontalAlignment.Center;

            //create labelOrario
            Label labelOrario = new Label();
            labelOrario.HorizontalAlignment = HorizontalAlignment.Left;
            labelOrario.Height = 31;
            labelOrario.Margin = new Thickness(3, 38, 0, 0);
            labelOrario.VerticalAlignment = VerticalAlignment.Top;
            labelOrario.Width = 153;
            labelOrario.VerticalContentAlignment = VerticalAlignment.Center;

            if (citta.Meteo.Informazioni != null)
            {
                try
                {
                    labelCitta.Content = citta.Nome;
                    image.Source = new BitmapImage(new Uri(citta.Meteo.Informazioni.current.condition.icon.Replace("//", "http://")));
                    labelTemp.Content = citta.Meteo.Informazioni.current.temp_c + "°";
                    labelOrario.Content = DateTime.Parse(citta.Meteo.Informazioni.location.localtime).ToString("HH:mm:ss");
                }
                catch (Exception)
                {
                    LogPersonalizzato.Log("Impossibile ricevere i dati. ");
                    MessageBox.Show("Impossibile ricevere i dati.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                grid.Children.Add(labelCitta);
                grid.Children.Add(image);
                grid.Children.Add(labelTemp);
                grid.Children.Add(labelOrario);
                return grid;
            }
            LogPersonalizzato.Log("Grid = null. Return grid.");
            return null;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            finestra = new AggiungereFusoOrario();
            ((Button)((Grid)(finestra.Content)).Children[2]).Click += CittaAggiunta;
            finestra.ShowDialog();
        }
        AggiungereFusoOrario finestra;
        private void CittaAggiunta(object sender, RoutedEventArgs e)
        {
            LogPersonalizzato.Log("Sto aggiungendo città.");
            string nome = ((TextBox)finestra.grid.Children[1]).Text;
            Citta citta = new Citta(nome);
            Grid nuovagrid = GetDefaultGrid(citta)/*.Result*/;
            if (nuovagrid == null) { return; }
            listaCitta.AddCitta(citta);
            listaGrid.Add(nuovagrid);
            gridd.Children.Add(nuovagrid);
            LogPersonalizzato.Log($"Città {citta} aggiunta.");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LogPersonalizzato.Log("Salvataggio dati.");
            File.WriteAllText(PATH_SALVATAGGIO_DATI_CITTA, listaCitta.ReceiveStringToSaveData());
            LogPersonalizzato.Log("Programma chiuso.");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LogPersonalizzato.Log("Caricamento dati.");
            if (!listaCitta.LoadCitiesData(PATH_SALVATAGGIO_DATI_CITTA)) return;
            List<Citta> lista = listaCitta.GetList();
            for (int i = 0; i < lista.Count; i++)
            {
                Grid grid = GetDefaultGrid(lista.ElementAt(i));
                listaGrid.Add(grid);
                gridd.Children.Add(grid);
                LogPersonalizzato.Log($"{(((i + 1) / (double)lista.Count) * 100).ToString("F1")}%");
            }
            //LogPersonalizzato.Log("Caricamento: 100%.");
        }
    }
}
