﻿using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerOpsaetning.Model
{
    public class Server : INotifyPropertyChanged
    {
        // Server client
        public SshClient client { get; set; }

        // Properties used in MainWindow.
        private string _serverIP;
        public string ServerIP
        {
            get { return _serverIP; }
            set
            {
                _serverIP = value;
                //OnPropertyChanged();
            }
        }
        private bool isServerOn;
        public bool IsServerOn
        {
            get { return isServerOn; }
            set
            {
                isServerOn = value;
                OnPropertyChanged("IsServerOn");
            }
        }

        // Properties used in ServerDetailsView.
        public string Uptime { get; set; }
        public string DiskSpace { get; set; }
        public string MemoryUsage{ get; set; }
        public string CpuUsage { get; set; }
        public string Password { get; set; }


        public Server(string IP, string username, string password, int port = 22)
        {
            Password = password;
            ServerIP = IP;
            client = new SshClient(IP, port, username, password);
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
                        Thread.Sleep(5000);
                        client.Connect();
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
