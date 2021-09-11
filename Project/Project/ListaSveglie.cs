using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class ListaSveglie
    {
        private List<Sveglia> sveglias;
        public ListaSveglie() { sveglias = new List<Sveglia>(); }
        public Sveglia GetSveglia(int index) { return sveglias.ElementAt(index); }
        public void AddSveglia(Sveglia citta) { sveglias.Add(citta); }
        public string ReceiveStringToSaveData()
        {
            string finale = "";
            foreach (Sveglia item in sveglias)
            {
                finale += $"{item.Nome};{item.Orario.ToString("G")};{item.Active}\n";
            }
            return finale;
        }
        public bool LoadSveglieDati(string path)
        {
            if (!File.Exists(path)) return false;
            string dati = File.ReadAllText(path);
            string[] sveglie = dati.Split('\n');
            if (sveglie.Length - 1 == 0) return false;
            for (int i = 0; i < sveglie.Length - 1; i++)
            {
                string[] datidivisi = sveglie[i].Split(';');
                AddSveglia(new Sveglia(datidivisi[0], DateTime.Parse(datidivisi[1]), bool.Parse(datidivisi[2])));
            }
            return true;
        }

        public int GetCount() { return sveglias.Count; }

        public bool RemoveSveglia(string nome)
        {
            foreach (Sveglia item in sveglias)
            {
                if (item.Nome == nome)
                {
                    sveglias.Remove(item);
                    return true;
                }
            }
            return false;
        }

        public List<Sveglia> GetList() { return sveglias; }
    }
}

