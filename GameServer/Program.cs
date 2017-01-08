using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            UDPServer server = new UDPServer(11000);
            server.Start();
        }
    }
}