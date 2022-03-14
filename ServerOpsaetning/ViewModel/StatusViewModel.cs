using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerOpsaetning.Model;
using Renci.SshNet;
using ServerOpsaetning.ViewModel;

namespace ServerOpsaetning.ViewModel
{
    public class StatusViewModel
    {
        public string Size { get; set; }
        private Server _server;
        public StatusViewModel()
        {
            _server = new Server("172.16.0.154", "cfe62qdf", "jona211x", 7373);
            GetStatus();
        }
        private async void GetStatus()
        {
            await Task.Factory.StartNew(() =>
            {
                SshCommand cmd = _server.client.CreateCommand("df-h");
                IAsyncResult result = cmd.BeginExecute();
                result.AsyncWaitHandle.WaitOne();
                var returnVal = cmd.EndExecute(result);
                result.AsyncWaitHandle.Close();
                Size = returnVal as string;
            });
        }
    }
}
