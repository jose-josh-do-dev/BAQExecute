using BAQExecute_WCF.DynamicQueryService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BAQExecute_WCF
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 1)
            {
                Environment.Exit(0);
            }
            ServicePointManager.ServerCertificateValidationCallback += (sender1, certificate, chain, errors) => { return true; };
            UriBuilder builder = new UriBuilder("https", $"{ Settings.Default.Host }" );
            builder.Path = $"/{Settings.Default.Instance}/Ice/BO/DynamicQuery.svc";
            builder.Port = Settings.Default.Port;
            using (var dynQry = EpicorClient.GetClient<DynamicQuerySvcContractClient, DynamicQuerySvcContract>(builder.Uri.ToString(), $"{Settings.Default.User}", $"{Settings.Default.Password}"))
            {
                dynQry.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(string.Empty, string.Empty));

                var eP = dynQry.GetQueryExecutionParametersByID(args[0]);
                bool morePages;
                var rslt = dynQry.GetListByID(args[0], eP, 0, 0, out morePages);
                foreach (DataRow r in rslt.Tables["Results"].Rows)
                {
                    r["RowMod"] = "U";
                }
                dynQry.UpdateByID(args[0], rslt);
            }
        }
    }
}
