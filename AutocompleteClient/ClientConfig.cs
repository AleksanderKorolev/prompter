using System;
using Prompter.Server;

namespace AutocompleteClient
{
    public static class ClientConfig
    {
        private static string serverAddress;
        private static int portNumber;

        public static void Load(string[] parameters)
        {
            if (parameters.Length < 2)
            {
                throw new ArgumentException(CommonMessages.WrongParametersDefinition);
            }
            serverAddress = parameters[0];
            string portValue = parameters[1];
            if (!int.TryParse(portValue, out portNumber))
            {
                throw new ArgumentException(CommonMessages.WrongPortNumberDefinition);
            }
        }

        public static string ServerAddress
        {
            get { return serverAddress; }
        }

        public static int PortNumber
        {
            get { return portNumber; }
        }
    }
}
