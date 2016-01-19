using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Chromefox
{
    class HttpClient : IDisposable
    {
        public string Host { get; }
        public int Port { get; }
        public Socket Socket { get; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {ReceiveTimeout = 3000};

        public HttpClient(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string HttpGetRequest()
        {
            try
            {
                Socket.Connect(Host, Port);
                Socket.Send(RequestBuilder.BuildRequest(Host));

                return ContentCutter.GetContentFromResponse(GetResponse());
            }
            catch (SocketException)
            {
                MessageBox.Show("Network fail!", "Error");

                Socket.Close();
                return string.Empty;
            }
        }

        private string GetResponse()
        {
            int bytes;
            var buf = new byte[512];
            var response = new StringBuilder();
            do
            {
                bytes = Socket.Receive(buf, buf.Length, 0);
                response.Append(Encoding.UTF8.GetString(buf));
            } while (bytes >= buf.Length);

            Socket.Close();
            return response.ToString();
        }

        public void Dispose() => Socket.Dispose();
    }
}
