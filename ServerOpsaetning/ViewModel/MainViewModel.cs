using Renci.SshNet;
using ServerOpsaetning.Model;
using ServerOpsaetning.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ServerOpsaetning.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<Server> serversCollection { get; set; }
        public RelayCommand MoreInfoCmd { get; set; }
        public Server server1 { get; set; }
        public MainViewModel()
        {
            server1 = new Server("192.168.1.179", "kasper", "kasper123"); // Bare skift værdierne så du kan se om din server virker.
            MoreInfoCmd = new RelayCommand(p => ViewServerDetails(server1));
            Trace.WriteLine(server1.IsServerOn);
            serversCollection = new ObservableCollection<Server>();
        }

        // Metode for at teste om man kan connecte til sin server.
        private void ConnectToServer(string connection, string username, string password)
        {
            //Server server = new Server(true, "192.168.1.179", "kasper", "kasper123");
            //serversCollection.Add(server);

        }

        private void ViewServerDetails(Server server)
        {
            ServerDetailsView sdv = new ServerDetailsView(server);
            sdv.ShowDialog();
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
