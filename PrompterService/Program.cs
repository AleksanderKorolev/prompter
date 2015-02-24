using System;
using log4net.Config;

namespace AutocompleteService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServerConfig.Load(args);
                XmlConfigurator.Configure();
                var statisticsOfWords = new StatisticsOfWords();
                statisticsOfWords.LoadStatistics();
                var server = new AutocompleteServer(statisticsOfWords.FrequencyOfWords);
                server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Нажмите клавишу для выхода из приложения...");
            Console.ReadLine();
        }
    }
}