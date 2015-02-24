using System;
using System.Linq;
using log4net;
using Prompter;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Prompter.Server;

namespace AutocompleteService
{
    public class AutocompleteServer
    {
        private readonly ManualResetEvent synchronizer = new ManualResetEvent(false);
        private readonly SyllableAnalysis SyllableAnalysis;
        private readonly ILog Log = LogManager.GetLogger("Log");
        private Socket serverListener;

        public AutocompleteServer(IDictionary<string, int> frequencyofWords)
        {
            SyllableAnalysis = new SyllableAnalysis(frequencyofWords);
        }

        public static IPAddress GetCurrentAddress()
        {
            IPHostEntry ipServerInfo = Dns.GetHostEntry(Dns.GetHostName());
            return ipServerInfo.AddressList.FirstOrDefault(serverAddress => serverAddress.AddressFamily == AddressFamily.InterNetwork);
        }

        public void Start()
        {
            IPAddress serverAddress = GetCurrentAddress();
            if (serverAddress == null)
            {
                throw new Exception("Невозможно получить текущий ip-адрес");
            }
            string startMessage = string.Format("Попытка запуска по адресу {0}:{1}", serverAddress, ServerConfig.PortNumber);
            InfoMessage(startMessage);
            Console.WriteLine(startMessage);

            var serverEndPoint = new IPEndPoint(serverAddress, ServerConfig.PortNumber);
            serverListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverListener.Bind(serverEndPoint);
                serverListener.Listen(1000);
                while (true)
                {
                    synchronizer.Reset();
                    serverListener.BeginAccept(AcceptCallback, serverListener);
                    synchronizer.WaitOne();
                }
            }
            catch (Exception exception)
            {
                string exceptionMessage = string.Format("В процесса обработки запроса произошло исключение {0}", exception);
                ErrorMessage(exceptionMessage);
                Console.WriteLine(exceptionMessage);
            }
            string exitMessage = "Нажмите любую клавишу для выхода из приложения...";
            InfoMessage(exitMessage);
            Console.WriteLine("\n{0}", exitMessage);
            Console.Read();
        }

        public void Stop()
        {
            serverListener.Close();
            Console.WriteLine("Завершение работы сервера");
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            synchronizer.Set();
            var listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            var exchange = new ExchangeObject { WorkSocket = handler };
            handler.BeginReceive(exchange.Buffer, 0, ExchangeObject.BufferSize, 0, ReadCallback, exchange);
        }

        private string Autocomplete(string syllable)
        {
            var result = SyllableAnalysis.Autocomplete(syllable);
            return string.Join(";", result.ToArray());
        }

        public void ReadCallback(IAsyncResult ar)
        {
            var exchangedObject = (ExchangeObject)ar.AsyncState;
            Socket handler = exchangedObject.WorkSocket;
            int bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0)
            {
                exchangedObject.ContentBuilder.Append(Encoding.ASCII.GetString(exchangedObject.Buffer, 0, bytesRead));
                string content = exchangedObject.ContentBuilder.ToString();
                if (content.IndexOf(CommonMessages.EndOfContent, StringComparison.InvariantCulture) > -1)
                {
                    string syllable = content.Replace(CommonMessages.EndOfContent, string.Empty);
                    string readMessage = string.Format("Считывание данных получателя: {0}", syllable);
                    InfoMessage(readMessage);
                    Console.WriteLine(readMessage);
                    string autocompletedResult = Autocomplete(syllable);
                    InfoMessage(string.Format("Слог {0}, слова автозаполнения: {1}", syllable, autocompletedResult));

                    Send(handler, autocompletedResult);
                }
                else
                {
                    handler.BeginReceive(exchangedObject.Buffer, 0, ExchangeObject.BufferSize, 0, ReadCallback, exchangedObject);
                }
            }
        }

        private void Send(Socket requestHandler, string data)
        {
            byte[] bytesToSend = Encoding.ASCII.GetBytes(data);
            requestHandler.BeginSend(bytesToSend, 0, bytesToSend.Length, 0, SendCallback, requestHandler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            var requestHandler = (Socket)ar.AsyncState;
            int bytesToSend = requestHandler.EndSend(ar);

            string sendMessage = string.Format("Отправка {0} байтов получателю", bytesToSend);
            Console.WriteLine(sendMessage);
            InfoMessage(sendMessage);

            requestHandler.Shutdown(SocketShutdown.Both);
            requestHandler.Close();
        }

        private void InfoMessage(string message)
        {
            if (ServerConfig.UseLog)
            {
                DateTime currentTime = DateTime.Now;
                Log.Info(string.Format("{0} {1}: {2}", currentTime.ToShortDateString(), currentTime.ToShortTimeString(), message));
            }
        }

        private void ErrorMessage(string message)
        {
            if (ServerConfig.UseLog)
            {
                DateTime currentTime = DateTime.Now;
                Log.Error(string.Format("{0} {1}: {2}", currentTime.ToShortDateString(), currentTime.ToShortTimeString(), message));
            }
        }
    }
}