using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerOpsaetning.Model
{
    class Server
    {
        public int ServerIP { get; set; }
        public bool IsServerOn { get; set; }
        public string ServerUsername { get; set; }
        public string ServerPassword { get; set; }

        public Server(int IP, bool IsOn, string username, string password)
        {
            ServerIP = IP;
            IsServerOn = IsOn;
            ServerUsername = username;
            ServerPassword = password;
        }
    }
}
