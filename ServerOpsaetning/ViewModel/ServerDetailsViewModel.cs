using ServerOpsaetning.Model;
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

        public Server server { get; set; }
        public ServerDetailsViewModel(Server server)
        {
            this.server = server;
            CreateGetInformationTask();
        }

        private void CreateGetInformationTask()
        {
            Task InfoTask = Task.Factory.StartNew(() => GetServerInformation(server));
        }

        private void GetServerInformation(Server server)
        {
            while (true)
            {
                try
                {
                    var UptimeCommand = server.client.RunCommand(@"uptime | awk -F'( |,|:)+' '{print$6,""minutes""}'");
                    var MemoryUsageCommand = server.client.RunCommand(@"free -m | grep ""Mem:"" | awk '{print $3,""MB used of"",$2,""MB""}'");
                    var CpuUsageCommand = server.client.RunCommand(@"top -b -n1 | grep ""Cpu(s)"" | awk '{print $2 + $4, ""%""}'");
                    var DiskUsageCommand = server.client.RunCommand(@"df -hx squashfs --total | grep ""total"" | awk '{print $3,""used of"",$2}'");
                    server.Uptime = string.Format(UptimeCommand.Result);
                    server.DiskSpace = string.Format(DiskUsageCommand.Result);
                    server.MemoryUsage= string.Format(MemoryUsageCommand.Result);
                    server.CpuUsage = string.Format(CpuUsageCommand.Result);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                OnPropertyChanged();
                Thread.Sleep(5000);
            }
        }

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
