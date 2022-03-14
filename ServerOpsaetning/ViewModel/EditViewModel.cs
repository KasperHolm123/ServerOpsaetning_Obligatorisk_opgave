﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using ServerOpsaetning.Model;

namespace ServerOpsaetning.ViewModel
{
    public delegate void ServerCreated(Server server);
    public class EditViewModel : ICloseable
    {
        public event EventHandler CloseRequest;
        public event ServerCreated created;
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RelayCommand EditCmd { get; set; }
        private Server _server;

        public EditViewModel(ServerCreated method)
        {
            created += method;
            EditCmd = new RelayCommand(p => Update(), p => CanEdit() );
        }

        private Task<Server> Edit()
        {
            return Task<Server>.Factory.StartNew(() =>
            {
                try
                {
                    _server = new Server(Host, Password, Username, Int32.Parse(Port));
                    _server.client.Connect();
                }
                catch(Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                return _server;
            });
        }

        private void Update()
        {
            Task<Server> task = Edit();
            Task.WaitAll();
            created.Invoke(task.Result);
            CloseRequest.Invoke(this, new EventArgs());
            
        }

        private bool CanEdit()
        {
            //Kan ikke være null, så fucker commands op, port skal være int
            return Host != null && Password != null && Username != null && Port != default;
        }
    }

    public interface ICloseable
    {
        event EventHandler CloseRequest;
    }
}
