﻿using ServerOpsaetning.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ServerOpsaetning.ViewModel
{
    class ServerDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> ProcessFound;
        public Server server { get; set; }
        public string ProcessesString { get; set; }
        public ServerDetailsViewModel(Server server, Action<string> action)
        {
            ProcessFound = action;
            this.server = server;
            CreateGetInformationTask();
            GetServerProcesses();
        }

        private void CreateGetInformationTask()
        {
            Task InfoTask = Task.Factory.StartNew(() => GetServerInformation());
        }

        private void GetServerInformation()
        {
            while (true)
            {
                try
                {
                    var UptimeCommand = server.client.RunCommand(@"uptime | awk -F'( |,|:)+' '{print$6,""minutes""}'");
                    server.Uptime = string.Format(UptimeCommand.Result);
                    var MemoryUsageCommand = server.client.RunCommand(@"free -m | grep ""Mem:"" | awk '{print $3,""MB used of"",$2,""MB""}'");
                    server.MemoryUsage= string.Format(MemoryUsageCommand.Result);
                    var CpuUsageCommand = server.client.RunCommand(@"top -b -n1 | grep ""Cpu(s)"" | awk '{print $2 + $4, ""%""}'");
                    server.CpuUsage = string.Format(CpuUsageCommand.Result);
                    var DiskUsageCommand = server.client.RunCommand(@"df -hx squashfs --total | grep ""total"" | awk '{print $3,""used of"",$2}'");
                    server.DiskSpace = string.Format(DiskUsageCommand.Result);
                    var ProcessesCommand = server.client.RunCommand(@"ps -aux | awk '{print $1,""  "",$2,"" "",$3,"" "",$4,"" "",$9,"" "",$10,"" "",$11,"" "",$12}'");
                    ProcessesString = ProcessesCommand.Result;
                    ProcessFound.Invoke(ProcessesString);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                OnPropertyChanged();
                Thread.Sleep(5000);
            }
        }

        private void GetServerProcesses()
        {
            var testCommand = server.client.RunCommand(@"ps -aux | awk '{print $1,""  "",$2,"" "",$3,"" "",$4,"" "",$9,"" "",$10,"" "",$11,"" "",$12}'");
            ProcessesString = testCommand.Result;
            ProcessFound.Invoke(ProcessesString);
        }

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
