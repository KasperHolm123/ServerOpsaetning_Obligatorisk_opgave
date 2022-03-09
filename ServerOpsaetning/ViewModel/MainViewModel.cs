using Renci.SshNet;
using ServerOpsaetning.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOpsaetning.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<Server> serversCollection { get; set; }
        public Server server1 { get; set; }
        public MainViewModel()
        {
            server1 = new Server("192.168.1.179", "kasper", "kasper123"); // Bare skift værdierne så du kan se om din server virker.
            Trace.WriteLine(server1.IsServerOn);
            serversCollection = new ObservableCollection<Server>();
        }

        // Metode for at teste om man kan connecte til sin server.
        private void ConnectToServer(string connection, string username, string password)
        {
            //Server server = new Server(true, "192.168.1.179", "kasper", "kasper123");
            //serversCollection.Add(server);

        }
    }
}
