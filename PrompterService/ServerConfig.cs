using System;
using System.IO;
using Prompter.Server;

namespace AutocompleteService
{
    public static class ServerConfig
    {
        private static string filePath;
        private static int portNumber;
        private static bool useLog = true;

        public static void Load(string[] parameters)
        {
            if (parameters.Length < 2)
            {
                throw new ArgumentException(CommonMessages.WrongParametersDefinition);
            }
            filePath = parameters[0];
            string portNumberValue = parameters[1];
            if (!File.Exists(filePath))
            {
                throw new ArgumentException(CommonMessages.WrongFilePathDefinition);
            }
            if (!int.TryParse(portNumberValue, out portNumber))
            {
                throw new ArgumentException(CommonMessages.WrongPortNumberDefinition);
            }
        }


        /// <summary>
        /// filepath to file
        /// </summary>
        public static string FilePath
        {
            get
            {
                return filePath;
            }
        }

        /// <summary>
        /// port number through which carried the interaction
        /// </summary>
        public static int PortNumber
        {
            get
            {
                return portNumber;
            }
        }

        /// <summary>
        /// determines whether to use the logging
        /// </summary>
        public static bool UseLog
        {
            get { return useLog;  }
            set { useLog = value; }
        }
    }
}