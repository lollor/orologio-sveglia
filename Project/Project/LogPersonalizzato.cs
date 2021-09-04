using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    static class LogPersonalizzato
    {
        public static string path;
        private static bool controlloIniziale = false;
        public static void Log(string message)
        {
            if (!controlloIniziale)
            {
                path = $@"logs\log_{DateTime.Now.ToString("dd_MM_yy_HH_mm_ss")}.txt";
                controlloIniziale = true;
            }
            File.AppendAllText(path, "(" + DateTime.Now + DateTime.Now.ToString(":fff") + ") " + message+ "\n");
        }
    }
}
