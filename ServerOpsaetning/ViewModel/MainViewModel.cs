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

namespace ServerOpsaetning.ViewModel
{
    class MainViewModel
    {
        public Server KasperServer { get; set; }
        public Server JonasServer { get; set; }
        public Server JohanServer { get; set; }
        public Server CustomServer { get; set; }

        public RelayCommand MoreInfoCmd { get; set; }
        public RelayCommand RebootServerCmd { get; set; }
        
        public MainViewModel()
        {
            KasperServer = new Server("192.168.1.179", "kasper", "kasper123", 7777);
            JonasServer = new Server("78.141.237.38", "root", "6rQ,%zZ!sy[UCu,r"); // Værdier skal ændres
            //JohanServer = new Server("temp", "temp", "temp", 7777); // Værdier skal ændres
            MoreInfoCmd = new RelayCommand(p => ViewMoreInfo((Server)p));
            RebootServerCmd = new RelayCommand(p => RebootServer((Server)p));
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

        private void RebootServer(Server server)
        {
            server.client.RunCommand("reboot");
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
