using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOpsaetning.ViewModel
{
    class MainViewModel
    {
        
        public MainViewModel()
        {
            ConnectToServer("192.168.9.179", "kasper", "kasper123");
        }

        private void ConnectToServer(string connection, string username, string password)
        {
            var connectionInfo = new ConnectionInfo("192.168.9.179", "kasper",
                                                    new PasswordAuthenticationMethod("kasper", "kasper123"));
            using (var client = new SshClient(connection, username, password))
            {
                client.Connect();
            }
        }
    }
}
