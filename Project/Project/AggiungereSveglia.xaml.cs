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

namespace Project
{
    /// <summary>
    /// Logica di interazione per AggiungereSveglia.xaml
    /// </summary>
    public partial class AggiungereSveglia : Window
    {
        public AggiungereSveglia()
        {
            InitializeComponent();
            calendario.SelectedDate = DateTime.Now;
            tbOra.Text = DateTime.Now.Hour.ToString();
            tbMinuto.Text = DateTime.Now.Minute.ToString();
        }

    }
}
