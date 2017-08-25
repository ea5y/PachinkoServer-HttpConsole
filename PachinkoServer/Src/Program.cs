using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace PachinkoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = IOHelper.ReadFromJson<Config>("./");
            DealRequest.InitConfig(config);
            HttpServer.Start(config);
        }
    }
}
