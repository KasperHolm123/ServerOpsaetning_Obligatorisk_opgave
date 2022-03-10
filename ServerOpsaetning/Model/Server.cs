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


        public Server(string IP, int port, string username, string password)
        {
            client = new SshClient(IP, port, username, password);
            try
            {
                client.Connect();
                var command1 = client.RunCommand("uptime");
                Trace.WriteLine(command1.Result);
                Trace.WriteLine("Connection attained.");
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
