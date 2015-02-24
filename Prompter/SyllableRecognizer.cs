using System;
using System.Collections.Generic;
using System.IO;

namespace Prompter
{
    public static class SyllableRecognizer
    {
        private static readonly char[] Delimiter = {' ', '\t'};

        public static KeyValuePair<string, int> RecognizeFrequencyOfWord(string recognizedString)
        {
            if (recognizedString == null)
            {
                throw new ArgumentException("Строка должна быть непустой");
            }
            // delimiter can be space and tab
            string[] inputedParts = recognizedString.Split(Delimiter, StringSplitOptions.None);
            if (inputedParts.Length != 2)
            {
                throw new ArgumentException(string.Format("Строка о частоте упоминания слова {0} задана некорректно", recognizedString));
            }
            int frequency;
            if (!int.TryParse(inputedParts[1], out frequency))
            {
                throw new ArgumentException(string.Format("Строка {0} имеет некорректный формат: строка должна представлять слово и частоту его употребления, разделенных пробелом", recognizedString));
            }
            return new KeyValuePair<string, int>(inputedParts[0], frequency);
        }

        public static IDictionary<string, int> RecognizeFrequencyOfWordFromArray(string[] recognizedContent)
        {
            int dictionaryLenght;
            if (!int.TryParse(recognizedContent[0], out dictionaryLenght))
            {
                throw new ArgumentException("Неправильно задано количество слов в словаре");
            }
            IDictionary<string, int> recognizedResult = new Dictionary<string, int>();
            for (int dictionaryIndex = 1; dictionaryIndex < dictionaryLenght + 1; dictionaryIndex++)
            {
                KeyValuePair<string, int> processedRow = RecognizeFrequencyOfWord(recognizedContent[dictionaryIndex]);
                recognizedResult.Add(processedRow);
            }
            return recognizedResult;
        }
    }
}