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
using System.IO.Ports;
using System.Windows.Shapes;

namespace Project
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string PATH_SALVATAGGIO_DATI_CITTA = "dati-citta.txt";
        private const string PATH_SALVATAGGIO_DATI_SVEGLIE = "dati-sveglie.txt";
        public ListaCitta listaCitta;
        public ListaSveglie listaSveglie;
        public string PortaCOM;
        SerialPort Serial;
        List<Grid> listaGridCitta, listaGridSveglie;
        public MainWindow()
        {
            listaGridCitta = new List<Grid>();
            listaGridSveglie = new List<Grid>();
            listaSveglie = new ListaSveglie();
            listaCitta = new ListaCitta();
            InitializeComponent();
            LogPersonalizzato.Log("Programma aperto.");
            SelezionaSerial selezionaSerial = new SelezionaSerial(this);
            selezionaSerial.ShowDialog();
            Serial = new SerialPort(PortaCOM);
            //Meteo meteo = new Meteo("Trezzano Rosa");
        }
        Grid GetDefaultCittaGrid(Citta citta)
        {
            LogPersonalizzato.Log("Inizio a creare grid.");
            Grid grid = new Grid();
            //Citta Citta = new Citta(citta);
            //Meteo meteo = new Meteo(citta);
            //grid settings
            grid.Height = 73;
            grid.Width = 267;
            grid.Background = Brushes.LightGray;
            grid.Margin = new Thickness(10, 38+listaGridCitta.Count*79, 0, 0);
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

        private void ButtonAggiungiCitta_Click(object sender, RoutedEventArgs e)
        {
            finestraFusoOrario = new AggiungereFusoOrario();
            ((Button)((Grid)(finestraFusoOrario.Content)).Children[2]).Click += CittaAggiunta;
            finestraFusoOrario.ShowDialog();
        }
        AggiungereFusoOrario finestraFusoOrario;
        private void CittaAggiunta(object sender, RoutedEventArgs e)
        {
            LogPersonalizzato.Log("Sto aggiungendo città.");
            string nome = ((TextBox)finestraFusoOrario.grid.Children[1]).Text;
            Citta citta = new Citta(nome);
            Grid nuovagrid = GetDefaultCittaGrid(citta)/*.Result*/;
            if (nuovagrid == null) { return; }
            listaCitta.AddCitta(citta);
            listaGridCitta.Add(nuovagrid);
            gridd.Children.Add(nuovagrid);
            LogPersonalizzato.Log($"Città {citta} aggiunta.");
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            LogPersonalizzato.Log("Salvataggio dati.");
            File.WriteAllText(PATH_SALVATAGGIO_DATI_CITTA, listaCitta.ReceiveStringToSaveData());
            File.WriteAllText(PATH_SALVATAGGIO_DATI_SVEGLIE, listaSveglie.ReceiveStringToSaveData());
            LogPersonalizzato.Log("Programma chiuso.");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LogPersonalizzato.Log("Caricamento dati città.");
            if (!listaCitta.LoadCitiesData(PATH_SALVATAGGIO_DATI_CITTA)) return;
            List<Citta> lista = listaCitta.GetList();
            for (int i = 0; i < lista.Count; i++)
            {
                Grid grid = GetDefaultCittaGrid(lista.ElementAt(i));
                listaGridCitta.Add(grid);
                gridd.Children.Add(grid);
                LogPersonalizzato.Log($"{(((i + 1) / (double)lista.Count) * 100).ToString("F1")}%");
            }
            //LogPersonalizzato.Log("Caricamento: 100%.");
            LogPersonalizzato.Log("Caricamento dati sveglie.");
            if (!listaSveglie.LoadSveglieDati(PATH_SALVATAGGIO_DATI_SVEGLIE)) return;
            List<Sveglia> sveglias = listaSveglie.GetList();
            for (int i = 0; i < sveglias.Count; i++)
            {
                Grid grid = GetDefaultSvegliaGrid(sveglias.ElementAt(i));
                listaGridSveglie.Add(grid);
                grid_sveglia.Children.Add(grid);
                LogPersonalizzato.Log($"{(((i + 1) / (double)lista.Count) * 100).ToString("F1")}%");
            }

            controllarePortaCOM();

        }
        private void controllarePortaCOM()
        {
            try
            {
                LogPersonalizzato.Log($"Provo ad aprire la porta in {PortaCOM}.");
                Serial.Open();
            }
            catch (Exception)
            {
                if (MessageBox.Show("Porta impossibile da aprire. Vuoi riprovare?", "Errore", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                    controllarePortaCOM();
                else
                    Environment.Exit(0);
            }
            LogPersonalizzato.Log("Porta aperta correttamente.");
        }

        Grid GetDefaultSvegliaGrid(Sveglia sveglia)
        {
            LogPersonalizzato.Log("Inizio a creare grid sveglia.");
            Grid grid = new Grid();
            //grid settings
            grid.Height = 73;
            grid.Width = 267;
            grid.Background = Brushes.LightGray;
            grid.Margin = new Thickness(10, 38 + listaGridSveglie.Count * 79, 0, 0);
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;

            //create labelNome
            Label labelNome = new Label();
            labelNome.HorizontalAlignment = HorizontalAlignment.Left;
            labelNome.Height = 31;
            labelNome.Margin = new Thickness(3, 5, 0, 0);
            labelNome.VerticalAlignment = VerticalAlignment.Top;
            labelNome.Width = 183;
            labelNome.VerticalContentAlignment = VerticalAlignment.Center;
            labelNome.FontWeight = FontWeights.Bold;


            //create checkbox
            CheckBox cbox = new CheckBox();
            cbox.HorizontalAlignment = HorizontalAlignment.Left;
            cbox.Height = 20;
            cbox.Margin = new Thickness(189, 27, 0, 0);
            cbox.VerticalAlignment = VerticalAlignment.Top;
            cbox.Width = 61;
            cbox.VerticalContentAlignment = VerticalAlignment.Center;
            cbox.Content = "Attiva";

            //create labelOrario
            Label labelOrario = new Label();
            labelOrario.HorizontalAlignment = HorizontalAlignment.Left;
            labelOrario.Height = 31;
            labelOrario.Margin = new Thickness(3, 38, 0, 0);
            labelOrario.VerticalAlignment = VerticalAlignment.Top;
            labelOrario.Width = 183;
            labelOrario.VerticalContentAlignment = VerticalAlignment.Center;

            if (sveglia != null)
            {
                try
                {
                    labelNome.Content = sveglia.Nome;
                    labelOrario.Content = $"{sveglia.Orario.ToString("g")}";
                    cbox.IsChecked = sveglia.Active;
                }
                catch (Exception)
                {
                    LogPersonalizzato.Log("Impossibile caricare i dati. ");
                    MessageBox.Show("Impossibile caricare i dati.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                grid.Children.Add(labelNome);
                grid.Children.Add(cbox);
                grid.Children.Add(labelOrario);
                return grid;
            }
            LogPersonalizzato.Log("Grid = null. Return grid.");
            return null;
        }
        AggiungereSveglia finestraSveglia;
        private void ButtonAggiungiSveglia_Click(object sender, RoutedEventArgs e)
        {
            finestraSveglia = new AggiungereSveglia();
            finestraSveglia.buttonAggiungi.Click += SvegliaAggiunta;
            finestraSveglia.ShowDialog();
        }

        private void SvegliaAggiunta(object sender, RoutedEventArgs e)
        {
            int ora, minuto;
            try
            {
                ora = Convert.ToInt32(finestraSveglia.tbOra.Text);
                minuto = Convert.ToInt32(finestraSveglia.tbMinuto.Text);
            } catch { return; }
            if (ora < 0 || ora > 23 || minuto < 0 || minuto > 59) return;
            if (finestraSveglia.tbNome.Text.Trim() == "") return;
            LogPersonalizzato.Log("Sto aggiungendo sveglia.");
            string nome = finestraSveglia.tbNome.Text;
            DateTime data = (DateTime)finestraSveglia.calendario.SelectedDate;
            data = new DateTime(data.Year, data.Month, data.Day, ora, minuto, 0);
            Sveglia sveglia = new Sveglia(nome, data);
            Grid nuovagrid = GetDefaultSvegliaGrid(sveglia)/*.Result*/;
            if (nuovagrid == null) { return; }
            listaSveglie.AddSveglia(sveglia);
            listaGridSveglie.Add(nuovagrid);
            grid_sveglia.Children.Add(nuovagrid);
            LogPersonalizzato.Log($"Sveglia {sveglia.Nome} aggiunta.");
        }

        private void ButtonEliminaSveglia_Click(object sender, RoutedEventArgs e)
        {
            EliminaSveglia window = new EliminaSveglia(this, PATH_SALVATAGGIO_DATI_SVEGLIE);
            window.ShowDialog();
        }

        private void ButtonEliminaCitta_Click(object sender, RoutedEventArgs e)
        {
            EliminaCitta window = new EliminaCitta(this, PATH_SALVATAGGIO_DATI_CITTA);
            window.ShowDialog();
        }
        private void ControllaSveglie()
        {

        }
    }
}
