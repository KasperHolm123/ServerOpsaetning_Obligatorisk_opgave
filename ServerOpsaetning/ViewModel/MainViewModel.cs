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
using System.Text.RegularExpressions;

namespace ServerOpsaetning.ViewModel
{
    public delegate void MessageEventHandler(Exception ex);

    public class MainViewModel : INotifyPropertyChanged
    {
        public event MessageEventHandler MessageHandler;
        public event PropertyChangedEventHandler PropertyChanged;

        private Server _centOSServer;
        private Server _ubuntuServer;
        private Server _debianServer;
        private Server _customServer;
        
        public RelayCommand MoreInfoCmd { get; set; }
        public RelayCommand EditCmd { get; set; }
        public RelayCommand RebootServerCmd { get; set; }

        public MainViewModel()
        {
            //KasperServer = new Server("192.168.1.179", "kasper", "kasper123", 7777);
            //JonasServer = new Server("172.16.0.154","jona211x", "cfe62qdf", 7373);
            //JohanServer = new Server("temp", "temp", "temp", 7777); // Værdier skal ændres
            MoreInfoCmd = new RelayCommand(p => ViewMoreInfo((Server)p));
            EditCmd = new RelayCommand(p => EditInfo());
            RebootServerCmd = new RelayCommand(p => RebootServer((Server)p));
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(10);
            //timer.Tick += ElapsedMethod;
            ConnectToServers();
        }

        private async Task ConnectToServers()
        {
            await Task.Factory.StartNew(() => CentOSServer = new Server("192.168.1.179", "kasper", "kasper123", 7777));
        }

        private void RebootServer(Server server)
        {
            using (var stream = server.client.CreateShellStream("tty1", 0, 0, 0, 0, 1024))
            {
                stream.WriteLine("sudo reboot");
                stream.Expect("password");
                stream.WriteLine(server.Password);
                var output = stream.Read();
            }
        }

        private void EditInfo()
        {
            Thread tr = new Thread(() => ViewEditServer());
            tr.SetApartmentState(ApartmentState.STA);
            tr.Start();
        }

        private void ViewEditServer()
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
            if (server != null)
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
                            if (MessageHandler != null)
                            {
                                MessageHandler(ex);
                            }
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
                            var command1 = server.client.RunCommand("ping -c 1 -q " + server.ServerIP);
                            if (command1.Result.Contains("0% packet loss")) return true;

                            else return false;
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandler != null)
                            {
                                MessageHandler(ex);
                            }
                        }
                        return server.client.IsConnected;
                    });
                }
            }
            return false;
        }

        private void OnPropertyChanged(string property = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async void ElapsedMethod(object sender, ElapsedEventArgs e)
        {
            //foreach server, if not null, getserverstate
            bool res = await GetServerState(CustomServer);
        }

        public Server CentOSServer
        {
            get { return _centOSServer; }
            set
            {
                _centOSServer = value;
                OnPropertyChanged();
            }
        }
        public Server UbuntuServer
        {
            get { return _ubuntuServer; }
            set
            {
                _ubuntuServer = value;
                OnPropertyChanged();
            }
        }
        public Server DebianServer
        {
            get { return _debianServer; }
            set
            {
                _debianServer = value;
                OnPropertyChanged();
            }
        }
        public Server CustomServer
        {
            get { return _customServer; }
            set
            {
                _customServer = value;
                OnPropertyChanged();
            }
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
