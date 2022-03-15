﻿using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOpsaetning.Model
{
    public class Server : INotifyPropertyChanged
    {
        // Server client
        public SshClient client { get; set; }
        
        // Properties used in MainWindow.
        public string ServerIP { get; set; }
        public bool IsServerOn
        {
            get { return isServerOn; }
            set
            {
                isServerOn = value;
                OnPropertyChanged("IsServerOn");
            }
        }
        private bool isServerOn;
        // Properties used in ServerDetailsView.
        

        public string Uptime { get; set; }
        public string DiskSpace { get; set; }
        public string MemoryUsage{ get; set; }
        public string CpuUsage { get; set; }
        public string Password { get; set; }


        public Server(string IP, string username, string password, int port = 22)
        {
            Password = password;
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
        public async Task GetServerState() //Try connect
        {
            if (!IsServerOn)
            {
                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        client.Connect();
                        var command1 = client.RunCommand("uptime");
                        Trace.WriteLine(command1.Result);
                        Trace.WriteLine("Connection attained.");
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Connection timed out.");
                    }
                    IsServerOn = client.IsConnected;
                });
            }
        }
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
