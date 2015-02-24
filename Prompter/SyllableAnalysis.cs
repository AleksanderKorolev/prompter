using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace Prompter
{
    public class SyllableAnalysis
    {
        private readonly IDictionary<string, int> frequencyOfWords = new Dictionary<string, int>();
        private readonly Dictionary<string, IEnumerable<string>> cachedFrequencyOfWords = new Dictionary<string, IEnumerable<string>>();
        private readonly Stopwatch stopWatch = new Stopwatch();

        public TimeSpan DurationOfTime
        {
            get { return stopWatch.Elapsed;  }
        }

        public int NumberOfWords
        {
            get { return frequencyOfWords.Count; }
        }

        public SyllableAnalysis(IDictionary<string, int> frequencyOfWords)
        {
            this.frequencyOfWords = frequencyOfWords;
        }

        public List<KeyValuePair<string, List<string>>> Autocomplete(List<string> syllables)
        {
            return syllables.Select(syllable => new KeyValuePair<string, List<string>>(syllable, Autocomplete(syllable))).ToList();
        }

        private string GetCachedKeyOfWordsBySyllable(string syllable)
        {
            return cachedFrequencyOfWords.Where(cfw => syllable.StartsWith(cfw.Key)).OrderByDescending(cfw => cfw.Key.Length).FirstOrDefault().Key;
        }

        public List<string> Autocomplete(string syllable)
        {
            if (string.IsNullOrEmpty(syllable))
            {
                return new List<string>();
            }
            stopWatch.Start();
            var cachedKey = GetCachedKeyOfWordsBySyllable(syllable);
            IEnumerable<string> orderedWords = cachedKey != null ? 
                                               cachedFrequencyOfWords[cachedKey] :
                                               frequencyOfWords.OrderByDescending(fw => fw.Value).ThenBy(fw => fw.Key).Select(fw => fw.Key);
            var result = (from word in orderedWords where word.StartsWith(syllable) select word).ToList();
            if (cachedKey == null || cachedKey != syllable)
            {
                cachedFrequencyOfWords.Add(syllable, result);
            }
            stopWatch.Stop();
            return result.GetRange(0, result.Count > 9 ? 10 : result.Count);
        }
    }
}