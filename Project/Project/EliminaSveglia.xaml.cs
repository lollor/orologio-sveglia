using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Project
{
    /// <summary>
    /// Logica di interazione per EliminaSveglia.xaml
    /// </summary>
    public partial class EliminaSveglia : Window
    {
        MainWindow window;
        public EliminaSveglia(MainWindow window, string path)
        {
            InitializeComponent();
            this.window = window;
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < window.listaSveglie.GetCount(); i++)
            {
                CheckBox box = new CheckBox()
                {
                    Height = 15,
                    Width = 238,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(12, 13 + i * 14, 0, 0),
                    IsChecked = true,
                    Content = window.listaSveglie.GetSveglia(i).Nome
                };
                gridd.Children.Add(box);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UIElementCollection collection = gridd.Children;
            foreach (var item in collection)
            {
                CheckBox check = (CheckBox)item;
                if (!(bool)check.IsChecked)
                    window.listaSveglie.RemoveSveglia((string)check.Content);
            }
            MessageBoxResult result = MessageBox.Show("E' necessario riavviare il programma per salvare le modifiche. Salvo e riavvio?", "Importante!", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                window.Window_Closed(null, null);
                try
                {
                    string startup = System.Reflection.Assembly.GetEntryAssembly().Location;
                    Process nuovo = Process.Start(startup);
                    Process.GetCurrentProcess().Kill();
                }
                catch { }
            }
            else if (result == MessageBoxResult.Cancel)
            {
                //annulla la chiusura
                e.Cancel = true;
            }
        }
    }
}
