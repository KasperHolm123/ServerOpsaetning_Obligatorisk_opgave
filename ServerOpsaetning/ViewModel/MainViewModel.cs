using Renci.SshNet;
using ServerOpsaetning.Model;
using ServerOpsaetning.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using System.Timers;
using System.ComponentModel;

namespace ServerOpsaetning.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Server KasperServer { get; set; }
        public Server JonasServer { get; set; }
        public Server JohanServer { get; set; }
        public Server CustomServer
        {
            get { return _customServer; }
            set
            {
                _customServer = value;
                OnPropertyChanged("CustomServer");
            }
        }
        private System.Timers.Timer timer;
        private Server _customServer;
        public RelayCommand MoreInfoCmd { get; set; }
        public RelayCommand EditCmd { get; set; }

        public MainViewModel()
        {
            JonasServer = new Server("172.16.0.154", 7373, "jona211x", "cfe62qdf");
            //KasperServer = new Server("192.168.1.179", 7777, "kasper", "kasper123"); // Værdier skal ændres
            //JohanServer = new Server("temp", 7777, "temp", "temp"); // Værdier skal ændres
            MoreInfoCmd = new RelayCommand(p => ViewMoreInfo((Server)p));
            EditCmd = new RelayCommand(p => EditInfo());
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += ElapsedEventHandler;
            timer.Start();
        }
        private void EditInfo()
        {
            Thread tr = new Thread(() => EditServer());
            tr.SetApartmentState(ApartmentState.STA);
            tr.Start();
        }

        private void EditServer()
        {
            CustomServerView csv = new CustomServerView(AddServer);
            csv.ShowDialog();
        }
        private void AddServer(Server server)
        {
            CustomServer = server;
        }
        // Metode for at teste om man kan connecte til sin server.
        private void ViewMoreInfo(Server server)
        {
            Thread DetailsThread = new Thread(() => ViewServerDetails(server));
            DetailsThread.SetApartmentState(ApartmentState.STA);
            DetailsThread.Start();

        }
        private void ViewServerDetails(Server server)
        {
            ServerDetailsView sdv = new ServerDetailsView(server);
            sdv.ShowDialog();
        }
        public async Task<bool> GetServerState(Server server) //Try connect
        {
            if (!server.IsServerOn)
            {
                return server.IsServerOn = await Task<bool>.Factory.StartNew(() =>
                {
                    try
                    {
                        server.client.Connect();
                        var command1 = server.client.RunCommand("uptime");
                        Trace.WriteLine(command1.Result);
                        Trace.WriteLine("Connection attained.");
                        server.ServerIP = server.client.ConnectionInfo.Host;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Connection timed out.");
                    }
                    return server.client.IsConnected;
                });
            }
            else
            {
                return server.IsServerOn = await Task<bool>.Factory.StartNew(() =>
                {
                    try
                    {
                        var command1 = server.client.RunCommand("ping -c 10 -q " + server.ServerIP);
                        if (command1.Result.Contains("0% packet loss")) return true;

                        else return false;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Connection timed out.");
                    }
                    return server.client.IsConnected;
                });
            }
        }
        private void OnPropertyChanged(string property)
        {
            if(PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private async void ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            //foreach server, if not null, getserverstate
            bool res = await GetServerState(JonasServer);
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
