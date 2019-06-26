using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAQExportImport_REST
{
    class Program
    {
        static void Main(string[] args)
        {
            SetAPISettings();
            //if(args.Length<)
        }

        static void SetAPISettings()
        {
            EpicorRestAPI.EpicorRest.AppPoolHost = $"{Settings.Default.Host}:{Settings.Default.Port}";
            EpicorRestAPI.EpicorRest.AppPoolInstance = Settings.Default.Instance;
            EpicorRestAPI.EpicorRest.UserName = Settings.Default.Username;
            EpicorRestAPI.EpicorRest.Password = Settings.Default.Password;
            EpicorRestAPI.EpicorRest.IgnoreCertErrors = true;
        }
    }
}
