using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BIIS
{
    /// <summary>
    /// Biis server class.
    /// </summary>
    public class BiisServer : IDisposable
    {
        private const string LocalhostIpAddress = "127.0.0.1";
        private readonly TcpListener _biisListener;

        /// <summary>
        /// Port, on which Biis server will be opened
        /// </summary>
        public int Port { get; }
        /// <summary>
        /// Folder, which Biis server will host
        /// </summary>
        public string WebRoot { get; }
        /// <summary>
        /// Biis server constructor
        /// </summary>
        /// <param name="port">Port, on which server will be opened</param>
        /// <param name="webRoot">Server hosting folder</param>
        public BiisServer(int port, string webRoot)
        {
            Port = port;
            WebRoot = webRoot;
            
            _biisListener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), Port);
        }
        /// <summary>
        /// Starts Biis server and process client request in multi-thread
        /// </summary>
        public void StartServer()
        {
            _biisListener.Start();

            while (true)
            {
                var client = _biisListener.AcceptTcpClient();

                // Pass each client in queue to threads, that are allocated in servers' thread pool
                ThreadPool.QueueUserWorkItem(state => { BiisClient.SendResponseToClient((TcpClient) state, WebRoot); }, client);
            }
        }
        /// <summary>
        /// Dispose method. Cleans all resources
        /// </summary>
        public void Dispose()
        {
            _biisListener.Stop();
            _biisListener.Server.Dispose();
        }
    }
}