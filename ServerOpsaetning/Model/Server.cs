using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOpsaetning.Model
{
    public class Server
    {
        // Server client
        public SshClient client { get; set; }
        
        // Properties used in MainWindow.
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public bool IsServerOn { get; set; }

        // Properties used in ServerDetailsView.
        public string Uptime { get; set; }
        public string DiskSpace { get; set; }
        public string MemoryUsage { get; set; }
        public string CpuUsage { get; set; }


        public Server(string IP, string username, string password, int port = 22)
        {
            client = new SshClient(IP, port, username, password);
            try
            {
                client.Connect();
                Trace.WriteLine("Connection established.");
                ServerIP = IP; // Only get the server IP if the connection has been established.
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Connection timed out.");
            }
            IsServerOn = client.IsConnected;
        }
    }
}
