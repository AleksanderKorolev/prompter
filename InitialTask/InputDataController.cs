using System;
using Prompter;
using System.IO;
using System.Collections.Generic;

namespace InitialTask
{
    public class InputDataController
    {
        private IDictionary<string, int> frequencyOfWords = new Dictionary<string, int>();
        private List<string> syllables = new List<string>();

        public IDictionary<string, int> FrequencyOfWords
        {
            get
            {
                if (frequencyOfWords == null)
                {
                    FillFrequencyOfWords();
                }
                return frequencyOfWords;
            }
        }

        public List<string> Syllables
        {
            get
            {
                if (syllables == null)
                {
                    FillSyllables();
                }
                return syllables;
            }
        }

        public void PerformUserInput()
        {
            FillFrequencyOfWords();
            FillSyllables();
        }

        public void PerformAutomaticInput(string filePath)
        {
            try
            {
                string[] fileContent = File.ReadAllLines(filePath);
                frequencyOfWords = SyllableRecognizer.RecognizeFrequencyOfWordFromArray(fileContent);
                int syllableCount;
                if (!int.TryParse(fileContent[frequencyOfWords.Count + 1], out syllableCount))
                {
                    throw new ArgumentException("Неправильно задано количество слогов");
                }
                for (int syllableIndex = frequencyOfWords.Count + 2; syllableIndex < fileContent.Length; syllableIndex++)
                {
                    syllables.Add(fileContent[syllableIndex]);
                }
            }
            catch (ArgumentException argException)
            {
                throw new ArgumentException("Некорректный формат входных данных", argException);
            }
            catch (Exception generalException)
            {
                throw new Exception("Некорректный формат входных данных", generalException);
            }
        }

        private int InputNumberOfRows()
        {
            string inputedNumber = Console.ReadLine();
            int number;
            if (!int.TryParse(inputedNumber, out number) || number <= 0)
            {
                throw new ArgumentException("Количество строк должно быть положительным числом");
            }
            return number;
        }

        private void FillSyllables()
        {
            int rowsCount = InputNumberOfRows();
            syllables = new List<string>();
            while (rowsCount > 0)
            {
                var inputedRow = Console.ReadLine();
                if (inputedRow == null)
                {
                    throw new ArgumentException("Строка должна быть непустой");
                }
                syllables.Add(inputedRow);
                rowsCount--;
            }
        }

        private void FillFrequencyOfWords()
        {
            try
            {
                int rowsCount = InputNumberOfRows();
                frequencyOfWords = new Dictionary<string, int>();
                while (rowsCount > 0)
                {
                    var inputedRow = Console.ReadLine();
                    KeyValuePair<string, int> processedRow = SyllableRecognizer.RecognizeFrequencyOfWord(inputedRow);
                    frequencyOfWords.Add(processedRow);
                    rowsCount--;
                }
            }
            // two different types of exceptions
            catch (ArgumentException argException)
            {
                throw new ArgumentException("Строка имеет неверный формат", argException);
            }
            catch (Exception generalException)
            {
                throw new Exception("Строка имеет неверный формат", generalException);
            }
        }
    }
}