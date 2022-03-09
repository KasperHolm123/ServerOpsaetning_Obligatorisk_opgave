using ServerOpsaetning.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        private void GetServerInformation(Server server)
        {
            var UptimeCommand = server.client.RunCommand(@"uptime | awk -F'( |,|:)+' '{print$6}'");
            server.Uptime = string.Format("{0} minutes", UptimeCommand.Result);
        }
    }
}
