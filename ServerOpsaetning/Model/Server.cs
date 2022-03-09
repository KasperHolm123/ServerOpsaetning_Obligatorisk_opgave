using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOpsaetning.Model
{
    class Server
    {
        public string ServerIP { get; set; }
        public bool IsServerOn { get; set; }

        public Server(string IP, string username, string password)
        {
            SshClient client = new SshClient(IP, username, password);
            try
            {
                client.Connect();
                ServerIP = IP;
                Trace.WriteLine("Connection attained.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Connection timed out.");
            }
            IsServerOn = client.IsConnected;
        }

        private void CreateConnection(string IP, string username, string password)
        {
            
        }
    }
}
