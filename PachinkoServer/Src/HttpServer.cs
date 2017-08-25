using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using YduCs;
using XY.Pachinko.CommunicationData;
using LitJson;

namespace PachinkoServer
{
    public class HttpServer
    {
        private static HttpListener listener;
        public static void Start(Config config)
        {
            using (HttpListener ls = new HttpListener())
            {
                listener = ls;
                listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

                //listener.Prefixes.Add("http://+:8080/");
                listener.Prefixes.Add(string.Format("http://+:{0}/", config.PORT));
                
                listener.Start();
                Console.WriteLine("WebServer Start Successed......");

                ThreadPoolMessage("MainThread");
                listener.BeginGetContext(ResponseThread, null);

                Console.ReadLine();
            }
        }

        private static void ThreadPoolMessage(string data)
        {
            int a, b;
            ThreadPool.GetAvailableThreads(out a, out b);
            string message = string.Format("\n{0}\n CurrentThread is {1}\n " +
                "WorkerThreads is: {2} CompletionPortThreads is: {3}",
                data, Thread.CurrentThread.ManagedThreadId, a, b);

            Console.WriteLine(message);
        }

        private static void ResponseThread(IAsyncResult ar)
        {
            listener.BeginGetContext(ResponseThread, null);
            ThreadPoolMessage("AsyncThread");

            var context = listener.EndGetContext(ar);

            Console.WriteLine(" Request:\n  Method:{0} URL:{1}", context.Request.HttpMethod, context.Request.Url);
            if (context.Request.HttpMethod == "GET")
                OnGet(context);
            else if (context.Request.HttpMethod == "POST")
                OnPost(context);
        }

        private static void OnGet(HttpListenerContext context)
        {
            //deal
            var res = DealRequest.Run(context);

            //callback
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write(res);
                
                writer.Close();
                context.Response.Close();
            }

            /*
            string data = context.Request.QueryString["data"];

            if (data != null)
            {
                Console.WriteLine("  Data:{0}", data);
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                var pachinkoRes = new PachinkoRes() { data = data };
                var pachinkoResJson = JsonMapper.ToJson(pachinkoRes);
                writer.Write(pachinkoResJson);
                
                writer.Close();
                context.Response.Close();
            }
            */

            //@TODO
            //Send TO DIO
        }

        private static void OnPost(HttpListenerContext context)
        {

        }
    }
}
