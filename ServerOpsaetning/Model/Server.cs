﻿using Renci.SshNet;
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
        public bool IsServerOn { get; set; }

        // Properties used in ServerDetailsView.
        public string Uptime { get; set; }
        public string DiskSpace { get; set; }
        public string RAMSpace { get; set; }
        public string CPUUsage { get; set; }


        public Server(string IP, string username, string password)
        {
            client = new SshClient(IP, username, password);
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
