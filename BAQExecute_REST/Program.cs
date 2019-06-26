using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAQExecute_REST
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 1)
            {
                Environment.Exit(0);
            }
            SetAPISettings();

            if (args.Count() == 2)
                EpicorRestAPI.EpicorRest.CallSettings = new EpicorRestAPI.CallSettings(args[1], string.Empty, string.Empty, string.Empty);


            dynamic dt = JsonConvert.DeserializeObject(EpicorRestAPI.EpicorRest.GetBAQResultJSON(args[0], new Dictionary<string, string>()));
            if (dt == null)
            {
                return;
            }
            foreach (dynamic dr in dt.value)
            {
                dr.RowMod = "U";
                EpicorRestAPI.EpicorRest.PatchBAQResults(args[0], dr);
            }
            

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
