using System.Text;

namespace Chromefox
{
    public class RequestBuilder
    {
        public static byte[] BuildRequest(string url) => Encoding.UTF8.GetBytes(new StringBuilder("GET / HTTP/1.1\r\n").
                                                                                       AppendLine($"Host: {url}").
                                                                                       AppendLine("Accept: */*").
                                                                                       AppendLine("Connection: Closed\r\n").ToString());
    }
}