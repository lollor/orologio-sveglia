using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Project
{
    public class Meteo
    {
        private const string KEY = "19cc6dfbdb74405c87c173313210209";
        private static HttpClient client = new HttpClient();
        private string dati;
        private string citta;
        private bool informazioniRicevute;
        private bool informazioniCorrette;
        public InfoRequest Informazioni { get; set; }
        public Meteo(string _citta)
        {
            LogPersonalizzato.Log($"Costruttore Meteo richiamato della citta {_citta}.");
            citta = _citta;
            informazioniRicevute = false;
            informazioniCorrette = false;
            LogPersonalizzato.Log("Chiamo la funzione DoRequest().");
            try
            {
                DoRequest();
            }
            catch (Exception ex)
            {
                LogPersonalizzato.Log($"Eccezione generata: {ex.Message}.");
            }
            
        }
        public bool IsInfoReceived() { return informazioniRicevute; }
        public bool IsInfoCorrect() { return informazioniCorrette; }
        async Task DoRequest()
        {
            LogPersonalizzato.Log("Inizio la richiesta.");
            citta.Replace(" ", "%20");
            try
            {
                //client.Timeout = TimeSpan.FromSeconds(1);
                HttpResponseMessage response = client.GetAsync("http://api.weatherapi.com/v1/current.json?key="+KEY+"&q="+citta+"&aqi=no").Result;
                LogPersonalizzato.Log("Risposta arrivata!");
                response.EnsureSuccessStatusCode();
                dati = response.Content.ReadAsStringAsync().Result;
                informazioniRicevute = true;
                informazioniCorrette = true;
                Console.WriteLine(dati);
                
            }
            catch (HttpRequestException e)
            {
                LogPersonalizzato.Log("Eccezione!!");
                LogPersonalizzato.Log($"Messaggio: {e.Message}.");
                LogPersonalizzato.Log(dati);
                MessageBox.Show("Messaggio: "+ e.Message,"Eccezione",MessageBoxButton.OK,MessageBoxImage.Warning);
                informazioniRicevute = true;
                informazioniCorrette = false;
                return;
            }
            LogPersonalizzato.Log("Converto i dati da JSON alla var Informazioni.");
            //LogPersonalizzato.Log(dati);
            Informazioni = JsonConvert.DeserializeObject<InfoRequest>(dati);
            LogPersonalizzato.Log("Dati convertiti.");
            //Informazioni.current.condition.icon.Remove(0, 2);
        }
        public class InfoRequest
        {
            public InfoLocation location;
            public InfoCurrent current;
            public class InfoLocation
            {
                public string name, region, country, localtime;
            }
            public class InfoCurrent
            {
                public float temp_c;
                public InfoCondition condition;
                public class InfoCondition
                {
                    public string text, icon;
                }
            }
        }
    }
}
