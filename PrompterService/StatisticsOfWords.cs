using System;
using System.Collections.Generic;
using System.IO;
using Prompter;

namespace AutocompleteService
{
    public class StatisticsOfWords
    {
        private IDictionary<string, int> frequencyOfWords = new Dictionary<string, int>();

        public IDictionary<string, int> FrequencyOfWords
        {
            get { return frequencyOfWords; }
        }

        public void LoadStatistics()
        {
            try
            {
                string[] fileContent = File.ReadAllLines(ServerConfig.FilePath);
                frequencyOfWords = SyllableRecognizer.RecognizeFrequencyOfWordFromArray(fileContent);
            }
            catch (Exception generalException)
            {
                throw new Exception("Невозможно открыть файл или файл содержит некорректные данные", generalException);
            }
        }
    }
}