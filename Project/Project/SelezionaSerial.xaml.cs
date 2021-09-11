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
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace Project
{
    /// <summary>
    /// Logica di interazione per SelezionaSerial.xaml
    /// </summary>
    public partial class SelezionaSerial : Window
    {
        MainWindow main;
        string porta;
        public SelezionaSerial(MainWindow main)
        {
            this.main = main;
            this.porta = "";
            InitializeComponent();
            ButtonAggiorna.Click += (object sender, RoutedEventArgs e) => { sistemaComboBox(null, null); };
        }


        private void sistemaComboBox(object sender, RoutedEventArgs e)
        {
            cbPortaCOM.Items.Clear();
            this.Cursor = Cursors.Wait;
            string[] ports = SerialPort.GetPortNames();
            foreach (string item in ports)
            {
                cbPortaCOM.Items.Add(item);
            }
            this.Cursor = Cursors.Arrow;
        }

        private void cbPortaCOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedIndex != -1)
            {
                SerialPort serial = new SerialPort((string)(box.SelectedItem));
                if (serial.IsOpen)
                {
                    MessageBox.Show("Impossibile aprire porta " + (string)(box.SelectedItem) + "\nScegliere un'altra porta", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                    cbPortaCOM.SelectedIndex = -1;
                } /*else
                {
                    porta = (string)(box.SelectedItem);
                }*/
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string pr = (string)(cbPortaCOM.SelectedItem);
            MessageBoxResult result;
            if (pr == "" || pr == null)
            {
                result = MessageBox.Show("Non puoi chiudere la finestra senza aver configurato la porta. Chiudere l'applicazione?", "Errore", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                Environment.Exit(0);
            }
            result = MessageBox.Show("Vuoi confermare questa porta?", "Informazione", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                main.PortaCOM = (string)(cbPortaCOM.SelectedItem);
                return;
            } else if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            } else
            {
                Environment.Exit(0);
            }
        }
    }
}
