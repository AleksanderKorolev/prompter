using System;

namespace AutocompleteClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ClientConfig.Load(args);
                while (true)
                {
                    string syllable = Console.ReadLine();
                    if (string.IsNullOrEmpty(syllable))
                    {
                        Console.WriteLine("Выход из программы");
                        return;
                    }
                    string[] commandParts = syllable.Split(' ');
                    if (commandParts.Length != 2 || commandParts[0] != "get" || string.IsNullOrEmpty(commandParts[1]))
                    {
                        Console.WriteLine("Для использования сервиса необходимо ввести команду вида get <prefix>");
                        continue;
                    }
                    var client = new AutocompleteClient();
                    client.SendRequest(commandParts[1]);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}