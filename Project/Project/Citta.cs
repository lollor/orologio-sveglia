using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project
{
    public class Citta
    {
        public string Nome { get; set; }
        public Meteo Meteo { get; private set; }
        public Citta(string nome)
        {
            Meteo = new Meteo(nome);
            while (!Meteo.IsInfoReceived() && !Meteo.IsInfoCorrect()) ;
            Nome = Meteo.Informazioni.location.name;
        }
    }
}
