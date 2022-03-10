using ServerOpsaetning.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerOpsaetning.ViewModel
{
    class ServerDetailsViewModel
    {
        public Server server { get; set; }
        public ServerDetailsViewModel(Server server)
        {
            this.server = server;
            GetServerInformation(server);
            //Task task1 = Task.Factory.StartNew(() => { GetServerInformation(server); });
        }

        private void GetServerInformation(Server server)
        {
            try
            {
                var UptimeCommand = server.client.RunCommand(@"uptime | awk -F'( |,|:)+' '{print$6}'");
                server.Uptime = string.Format("{0} minutes", UptimeCommand.Result);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
