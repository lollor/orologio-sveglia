using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Project
{
    public class Sveglia
    {
        public string Nome { get; set; }
        public DateTime Orario { get; private set; }
        public bool Active { get; private set; }
        public CheckBox checkBox { get; set; }
        public Sveglia(string nome, DateTime orario)
        {
            Nome = nome;
            Orario = orario;
            Active = true;
        }
        public Sveglia(string nome, DateTime orario, bool active)
        {
            Nome = nome;
            Orario = orario;
            Active = active;
        }
        public void AggiungiCheckBox(in CheckBox checkBox)
        {
            this.checkBox = checkBox;
        }
        public void ModificaValoreCheckBox(bool IsChecked)
        {
            checkBox.IsChecked = IsChecked;
            Active = IsChecked;
        }
        public bool CheckBoxIsChecked()
        {
            Console.WriteLine($"CheckBox sveglia {Nome}: {(bool)checkBox.IsChecked}");
            return (bool)checkBox.IsChecked;
        }
    }
}
