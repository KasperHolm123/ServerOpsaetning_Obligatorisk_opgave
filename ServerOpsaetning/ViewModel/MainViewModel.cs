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
            // Instantiate commands
            MoreInfoCmd = new RelayCommand(p => ViewMoreInfo((Server)p));
            EditCmd = new RelayCommand(p => EditInfo());
            RebootServerCmd = new RelayCommand(p => RebootServer((Server)p));
            // Instantiate servers
            CentOSServer = new Server("78.141.232.109", "root", "+1aEX5C)pP22a!,1");
            UbuntuServer = new Server("78.141.237.38", "root", "6rQ,%zZ!sy[UCu,r");
            DebianServer = new Server("45.32.176.122", "root", "g2A$B[n@x7uUKj-G");
            // Method calls
            ConnectToServers();
            // Continously check server connections
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += CheckServerConnectionAsync;
            timer.Start();
        }

        private void ConnectToServers()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => // StackOverFlow & LinkedIn foreslår at bruge dispatcher
            {
                _ = CentOSServer.GetServerState(); // Intellisense mener at discards er gode her. Aner ikke hvorfor.
                _ = UbuntuServer.GetServerState();
                _ = DebianServer.GetServerState();
            });
            
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

        private async void CheckServerConnectionAsync(object sender, EventArgs e)
        {
            if (CustomServer != null)
            {
                if (!CustomServer.client.IsConnected)
                {
                    await Task.Factory.StartNew(() => CustomServer.GetServerState());
                    CustomServer.IsServerOn = CustomServer.client.IsConnected;
                }
            }
            if (!CentOSServer.client.IsConnected)
            {
                await Task.Factory.StartNew(() => CentOSServer.GetServerState());
                CentOSServer.IsServerOn = CentOSServer.client.IsConnected;
            }
            if (!UbuntuServer.client.IsConnected)
            {
                await Task.Factory.StartNew(() => UbuntuServer.GetServerState());
                UbuntuServer.IsServerOn = UbuntuServer.client.IsConnected;
            }
            if (!DebianServer.client.IsConnected)
            {
                await Task.Factory.StartNew(() => DebianServer.GetServerState());
                DebianServer.IsServerOn = DebianServer.client.IsConnected;
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
            CustomServerView csv = new CustomServerView();
            if (csv.ShowDialog() == false)
            {
                CustomServer = csv.model.Server1;
            }
        }

        // Metode for at teste om man kan connecte til sin server.
        private void ViewMoreInfo(Server server)
        {
            try
            {
                Thread DetailsThread = new Thread(() => ViewServerDetails(server));
                DetailsThread.SetApartmentState(ApartmentState.STA);
                DetailsThread.Start();
            }
            catch (Exception ex)
            {
                if (MessageHandler != null)
                {
                    MessageHandler(ex);
                }
            }
        }

        private void ViewServerDetails(Server server)
        {
            ServerDetailsView sdv = new ServerDetailsView(server);
            sdv.ShowDialog();
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
        private void OnPropertyChanged(string property = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
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
