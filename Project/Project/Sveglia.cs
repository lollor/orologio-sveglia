using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Sveglia
    {
        public string Nome { get; set; }
        public DateTime Orario { get; private set; }
        public bool Active { get; set; }
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
    }
}
