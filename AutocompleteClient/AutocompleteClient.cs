using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Prompter.Server;

namespace AutocompleteClient
{
    public class AutocompleteClient
    {
        private readonly ManualResetEvent connectSynchronizer = new ManualResetEvent(false);
        private readonly ManualResetEvent sendSynchronizer = new ManualResetEvent(false);
        private readonly ManualResetEvent receiveSynchronizer = new ManualResetEvent(false);
        private string response;

        public List<string> SendRequest(string syllable)
        {
            connectSynchronizer.Reset();
            sendSynchronizer.Reset();
            receiveSynchronizer.Reset();
            response = string.Empty;
                
            IPAddress serverAddress = IPAddress.Parse(ClientConfig.ServerAddress);
            var serverEndPoint = new IPEndPoint(serverAddress, ClientConfig.PortNumber);
            var client = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.BeginConnect(serverEndPoint, ConnectCallback, client);
            connectSynchronizer.WaitOne();
            Send(client,string.Format("{0}{1}", syllable, CommonMessages.EndOfContent));
            sendSynchronizer.WaitOne();

            Receive(client);
            receiveSynchronizer.WaitOne();
                
            Console.WriteLine("Получен ответ с сервера: {0}", response);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            return response.Split(' ').ToList();
        }

        private void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
        }

        private void Receive(Socket client)
        {
            var exchangeObject = new ExchangeObject { WorkSocket = client };
            client.BeginReceive(exchangeObject.Buffer, 0, ExchangeObject.BufferSize, 0, ReceiveCallback, exchangeObject);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            var clientSocket = (Socket)ar.AsyncState;
            clientSocket.EndConnect(ar);
            Console.WriteLine("Попытка установить соединение с {0}", clientSocket.RemoteEndPoint);
            connectSynchronizer.Set();
        }

        private void SendCallback(IAsyncResult ar)
        {
            var clientSocket = (Socket)ar.AsyncState;
            int bytesSent = clientSocket.EndSend(ar);
            Console.WriteLine("Отправка {0} байтов на сервер", bytesSent);
            sendSynchronizer.Set();
        }

        private void ReceiveCallback(IAsyncResult ar) 
        {
            var exchangeObject = (ExchangeObject) ar.AsyncState;
            Socket client = exchangeObject.WorkSocket;
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0) 
            {
                exchangeObject.ContentBuilder.Append(Encoding.ASCII.GetString(exchangeObject.Buffer,0,bytesRead));
                client.BeginReceive(exchangeObject.Buffer,0,ExchangeObject.BufferSize,0, ReceiveCallback, exchangeObject);
            } 
            else
            {
                if (exchangeObject.ContentBuilder.Length > 1)
                {
                    response = exchangeObject.ContentBuilder.ToString();
                }
                receiveSynchronizer.Set();
            }
        }
    }
}