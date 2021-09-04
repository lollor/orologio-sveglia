using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class ListaCitta
    {
        private List<Citta> cittas;
        public ListaCitta() { cittas = new List<Citta>(); }
        public Citta GetCitta(int index) { return cittas.ElementAt(index); }
        public void AddCitta(Citta citta) { cittas.Add(citta); }
        public string ReceiveStringToSaveData()
        {
            string finale = "";
            foreach (Citta item in cittas)
            {
                finale += $"{item.Nome};";
            }
            return finale;
        }
        public bool LoadCitiesData(string path)
        {
            if (!File.Exists(path)) return false;
            string dati = File.ReadAllText(path);
            string[] datidivisi = dati.Split(';');
            if (datidivisi.Length - 1 == 0) return false;
            for (int i = 0; i < datidivisi.Length-1; i++)
            {
                AddCitta(new Citta(datidivisi[i]));
            }
            return true;
        }
        public List<Citta> GetList() { return cittas; }
    }
}
