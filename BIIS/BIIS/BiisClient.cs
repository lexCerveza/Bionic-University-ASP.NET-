using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using BIIS.Identities;

namespace BIIS
{
    /// <summary>
    /// Biis client helper class, makes treatment of client requests
    /// </summary>
    public class BiisClient
    {
        /// <summary>
        /// Sends reponse on GET and POST requests
        /// </summary>
        /// <param name="client"></param>
        /// <param name="webRoot"></param>
        public static void SendResponseToClient(TcpClient client, string webRoot)
        {
            // Get client request message
            var request = ReadClientRequest(client);   
            // Parse request string and get verb and url
            var requestMatch = Regex.Match(request, @"^(?<verb>\w*)\s*(?<url>[^\s]*)\s*HTTP/.*", RegexOptions.IgnoreCase);

            // If wrong request, send 400 error response
            if (requestMatch == Match.Empty)
            {
                SendError(client, HttpStatusCodesEnum.BadRequestError);
                return;
            }

            // parse HTTP request, fetch verb and url
            var verb = requestMatch.Groups["verb"].ToString();
            var url = requestMatch.Groups["url"].ToString();

            var filePath = $"{webRoot}\\{Regex.Replace(url, @"\S+/", string.Empty)}";
            // Handle requests, according to verb
            switch (verb)
            {
                case "GET":
                    HandleGetRequest(client, filePath);
                    break;
                case "POST":
                    HandlePostRequest(client, filePath);
                    break;
            }
        }

        /// <summary>
        /// Handles GET request from client and sends response
        /// </summary>
        /// <param name="client"></param>
        /// <param name="filePath"></param>
        private static void HandleGetRequest(TcpClient client, string filePath)
        {
            // if file path ends with slash (default web page), so it's index.html
            if (filePath.EndsWith("\\")) { filePath = $"{filePath}index.html"; }

            // if file doesn't exist - 404 error
            if (!File.Exists(filePath))
            {
                SendError(client, HttpStatusCodesEnum.NotFoundError);
                return;
            }

            // check extension of the file and make response to client
            var fileExtension = Regex.Match(filePath, @"(?<extension>\.\w+)").Groups["extension"].Value;
            var responseContents = string.Empty;
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                    responseContents = File.ReadAllText(filePath);
                    break;
                case ".biis":
                    responseContents = new BiisPage(File.ReadAllText(filePath), Program.Button_OnClick).HtmlPage;
                    break;
                default:
                    SendError(client, HttpStatusCodesEnum.InternalServerError);
                    break;
            }

            // full response to client
            const int intCode = (int) HttpStatusCodesEnum.Ok;
            var response = $"HTTP/1.1 {intCode} {GetHttpResponseCodeDescription(HttpStatusCodesEnum.Ok)}\nContent-Type: text/html\nContent-Length: {responseContents.Length}\n\n{responseContents}";
            var responseBytes = Encoding.UTF8.GetBytes(response);

            // send reponse to client
            client.GetStream().Write(responseBytes, 0, responseBytes.Length);
            client.Close();
        }
        /// <summary>
        /// Handles POST request from client and sends response
        /// </summary>
        private static void HandlePostRequest(TcpClient client, string filePath)
        {
            // if file doesn't exist - 404 error
            if (!File.Exists(filePath))
            {
                SendError(client, HttpStatusCodesEnum.NotFoundError);
                return;
            }

            // check extension of file
            var fileExtension = Regex.Match(filePath, @"(?<extension>\.\w+)").Groups["extension"].Value;
            BiisPage biisPage;
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                    // I'm so tired, I can't implement html handler. Sorry(
                    SendError(client, HttpStatusCodesEnum.InternalServerError);
                    return;
                case ".biis":
                    biisPage = new BiisPage(File.ReadAllText(filePath), Program.Button_OnClick);
                    break;
                default:
                    SendError(client, HttpStatusCodesEnum.InternalServerError);
                    return;
            }

            var responseContents = biisPage.GetHtmlAfterPost();
            const int intCode = (int)HttpStatusCodesEnum.Ok;
            var response = $"HTTP/1.1 {intCode} {GetHttpResponseCodeDescription(HttpStatusCodesEnum.Ok)}\nContent-Type: text/html\nContent-Length: {responseContents.Length}\n\n{responseContents}";
            var responseBytes = Encoding.UTF8.GetBytes(response);

            // send reponse to client
            client.GetStream().Write(responseBytes, 0, responseBytes.Length);
            client.Close();
        }
        /// <summary>
        /// Reads request from client using buffering
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static string ReadClientRequest(TcpClient client)
        {
            var requestBuilder = new StringBuilder();
            int bytes;
            var buf = new byte[64];

            // Read client request
            while ((bytes = client.GetStream().Read(buf, 0, buf.Length)) > 0)
            {
                requestBuilder.Append(Encoding.UTF8.GetString(buf, 0, bytes));

                // if current buffer contains http request escape characters, end reading
                if (Regex.Match(requestBuilder.ToString(), @"\r\n\r\n").Success)
                {
                    break;
                }
            }
            return requestBuilder.ToString();
        }
        /// <summary>
        /// Sends error response to client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="code"></param>
        private static void SendError(TcpClient client, HttpStatusCodesEnum code)
        {
            var intCode = (int) code;
            var responseCode = $"{intCode} {GetHttpResponseCodeDescription(code)}\n";
            var html = $"<html><body><h1>{responseCode}</h1></body></html>";

            var clientResponse = Encoding.UTF8.GetBytes($"HTTP/1.1 {responseCode}Content-Type: text/html\nContent-Length:{html.Length}\n\n{html}");
            client.GetStream().Write(clientResponse, 0, clientResponse.Length);
            client.Close();
        }
        /// <summary>
        /// Returns description of HttpStatusCodesEnum (using reflection)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetHttpResponseCodeDescription(HttpStatusCodesEnum code)
        {
            var type = code.GetType();
            // fetch public members of enum
            var memberInfo = type.GetMember(code.ToString());

            if (memberInfo.Length <= 0) return code.ToString();

            // fetch attributes from this public members
            var attributes = memberInfo[0].GetCustomAttributes(typeof (DescriptionAttribute), false);
            // get content of attribute
            return attributes.Length > 0 ? ((DescriptionAttribute) attributes[0]).Description : code.ToString();
        }
    }
}