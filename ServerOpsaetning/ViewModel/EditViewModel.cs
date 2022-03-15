using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using ServerOpsaetning.Model;

namespace ServerOpsaetning.ViewModel
{
    public delegate void ServerCreated(Server server);
    public class EditViewModel : ICloseable, INotifyPropertyChanged
    {
        public event EventHandler CloseRequest;
        public event ServerCreated created;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RelayCommand EditCmd { get; set; }
        private Server _server;
        public EditViewModel(ServerCreated server)
        {
            created += server;
            EditCmd = new RelayCommand(p => Update(), p => CanEdit());
        }
        private Task<Server> Edit()
        {
            return Task<Server>.Factory.StartNew(() =>
            {
                try
                {
                    Server1 = new Server(Host, Username, Password, Int32.Parse(Port));
                    Server1.client.Connect();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                Server1.IsServerOn = Server1.client.IsConnected;
                return Server1;
            });
        }
        private void Update()
        {
            Task<Server> task = Edit();
            task.Wait();
            created.Invoke(task.Result);
            CloseRequest.Invoke(this, new EventArgs());
        }
        private bool CanEdit()
        {
            //Kan ikke være null, så fucker commands op, port skal være int
            return Host != null && Password != null && Username != null && Port != default;
        }

        public Server Server1
        {
            get { return _server; }
            set
            {
                _server = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged(string property = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    public interface ICloseable
    {
        event EventHandler CloseRequest;
    }
}

