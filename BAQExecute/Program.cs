using Epicor.ServiceModel.Channels;
using Epicor.ServiceModel.StandardBindings;
using Ice.Contracts;
using Ice.Proxy.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace BAQExecute
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 1)
            {
                Environment.Exit(0);
            }
            CustomBinding binding = NetTcp.UsernameWindowsChannel();
            if(args.Length!=0)
            using (var dynQry = GetEpicorBOInstance<DynamicQueryImpl, DynamicQuerySvcContract>(binding, new Uri($"net.tcp://{Settings.Default.Server}/{Settings.Default.Instance}/ICE/BO/DynamicQuery.svc"), Settings.Default.UserName, Settings.Default.Password))
            {
                    var eP = dynQry.GetQueryExecutionParametersByID(args[0]);
                    bool morePages;
                    var rslt = dynQry.GetListByID(args[0], eP, 0, 0, out morePages);
                    foreach(DataRow r in rslt.Tables["Results"].Rows)
                    {
                        r["RowMod"] = "U";
                    }
                    dynQry.UpdateByID(args[0], rslt);
            }
        }
        public static TImpl GetEpicorBOInstance<TImpl, TContract>(CustomBinding binding, Uri appServer, string userName, string password, EndpointIdentity epi = null)
            where TImpl : ImplBase<TContract>
            where TContract : class
        {

            TImpl client = (TImpl)Activator.CreateInstance(typeof(TImpl), binding, appServer, epi);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;

            return client;
        }
    }
}
