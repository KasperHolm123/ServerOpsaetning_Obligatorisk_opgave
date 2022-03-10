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
            //GetServerInformation(server);
            tempMethod();
        }


        private void tempMethod()
        {
            Task InfoTask = Task.Factory.StartNew(() => GetServerInformation(server));
        }

        private void GetServerInformation(Server server)
        {
            while (true)
            {
                try
                {
                    var UptimeCommand = server.client.RunCommand(@"uptime | awk -F'( |,|:)+' '{print$8,"":""$7, ""minutes""}'");
                    server.Uptime = string.Format(UptimeCommand.Result);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
