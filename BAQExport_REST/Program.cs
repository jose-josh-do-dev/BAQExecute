using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAQExport_REST
{
    class Program
    {
        static void Main(string[] args)
        {
            SetAPISettings();
            if (args.Length < 2)
            {
                Console.WriteLine("<BAQID> <EXPORTPATH> Arguments Required");
                Console.ReadLine();
            }
            else
            {
                DataTable dt = EpicorRestAPI.EpicorRest.GetBAQResults(args[0], null);
                StringBuilder sb = new StringBuilder();

                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(",", fields));
                }

                File.WriteAllText($"{args[1]}{args[0]}-{Guid.NewGuid()}.csv", sb.ToString());
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
