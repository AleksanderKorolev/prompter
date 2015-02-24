using Prompter;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestByFiltering()
        {
            var frequencyOfWords = new Dictionary<string, int>
            {
                {"kawaii", 25},
                {"kare", 7},
                {"korosu", 20},
                {"sakura", 3}
            };
            var syllable = new List<string>
            {
                "k",
                "ka", 
                "kaw"
            };
            var syllableAnalysis = new SyllableAnalysis(frequencyOfWords);
            List<KeyValuePair<string, List<string>>> autocompleteResults = syllableAnalysis.Autocomplete(syllable);

            Assert.That(autocompleteResults[0].Key, Is.EqualTo("k"));
            Assert.That(autocompleteResults[0].Value.Count, Is.EqualTo(3));
            Assert.That(autocompleteResults[0].Value[0], Is.EqualTo("kawaii"));
            Assert.That(autocompleteResults[0].Value[1], Is.EqualTo("korosu"));
            Assert.That(autocompleteResults[0].Value[2], Is.EqualTo("kare"));

            Assert.That(autocompleteResults[1].Key, Is.EqualTo("ka"));
            Assert.That(autocompleteResults[1].Value.Count, Is.EqualTo(2));
            Assert.That(autocompleteResults[1].Value[0], Is.EqualTo("kawaii"));
            Assert.That(autocompleteResults[1].Value[1], Is.EqualTo("kare"));

            Assert.That(autocompleteResults[2].Key, Is.EqualTo("kaw"));
            Assert.That(autocompleteResults[2].Value.Count, Is.EqualTo(1));
            Assert.That(autocompleteResults[2].Value[0], Is.EqualTo("kawaii"));
        }

        /// <summary>
        /// Test for frequency of words
        /// </summary>
        [Test]
        public void TestOrderWordsByFrequency()
        {
            var frequencyOfWords = new Dictionary<string, int>
            {
                {"kanojo", 1},
                {"karetachi", 10},
                {"korosu", 20},
                {"kare", 7},
                {"kawaii", 25}
            };
            var syllables = new List<string>
            {
                "k",
                "ka",
                "kar"
            };
            var syllableAnalysis = new SyllableAnalysis(frequencyOfWords);
            List<KeyValuePair<string, List<string>>> autocompleteResults = syllableAnalysis.Autocomplete(syllables);

            Assert.That(autocompleteResults[0].Key, Is.EqualTo("k"));
            Assert.That(autocompleteResults[0].Value.Count, Is.EqualTo(5));
            Assert.That(autocompleteResults[0].Value[0], Is.EqualTo("kawaii"));
            Assert.That(autocompleteResults[0].Value[1], Is.EqualTo("korosu"));
            Assert.That(autocompleteResults[0].Value[2], Is.EqualTo("karetachi"));
            Assert.That(autocompleteResults[0].Value[3], Is.EqualTo("kare"));
            Assert.That(autocompleteResults[0].Value[4], Is.EqualTo("kanojo"));

            Assert.That(autocompleteResults[1].Key, Is.EqualTo("ka"));
            Assert.That(autocompleteResults[1].Value.Count, Is.EqualTo(4));
            Assert.That(autocompleteResults[1].Value[0], Is.EqualTo("kawaii"));
            Assert.That(autocompleteResults[1].Value[1], Is.EqualTo("karetachi"));
            Assert.That(autocompleteResults[1].Value[2], Is.EqualTo("kare"));
            Assert.That(autocompleteResults[1].Value[3], Is.EqualTo("kanojo"));

            Assert.That(autocompleteResults[2].Key, Is.EqualTo("kar"));
            Assert.That(autocompleteResults[2].Value.Count, Is.EqualTo(2));
            Assert.That(autocompleteResults[2].Value[0], Is.EqualTo("karetachi"));
            Assert.That(autocompleteResults[2].Value[1], Is.EqualTo("kare"));
        }

        /// <summary>
        /// Test of ordering by words
        /// </summary>
        [Test]
        public void TestOrderByWords()
        {
            var frequencyOfWords = new Dictionary<string, int>
            {
                {"korosu", 10},  
                {"kawaii", 10},
                {"kanojo", 10},
                {"karetachi", 10},
                {"kare", 10}
            };
            var syllables = new List<string>
            {
                "k",
                "ka",
                "kar"
            };
            var syllableAnalysis = new SyllableAnalysis(frequencyOfWords);
            List<KeyValuePair<string, List<string>>> autocompleteResult = syllableAnalysis.Autocomplete(syllables);

            Assert.That(autocompleteResult[0].Key, Is.EqualTo("k"));
            Assert.That(autocompleteResult[0].Value.Count, Is.EqualTo(5));
            Assert.That(autocompleteResult[0].Value[0], Is.EqualTo("kanojo"));
            Assert.That(autocompleteResult[0].Value[1], Is.EqualTo("kare"));
            Assert.That(autocompleteResult[0].Value[2], Is.EqualTo("karetachi"));
            Assert.That(autocompleteResult[0].Value[3], Is.EqualTo("kawaii"));
            Assert.That(autocompleteResult[0].Value[4], Is.EqualTo("korosu"));

            Assert.That(autocompleteResult[1].Key, Is.EqualTo("ka"));
            Assert.That(autocompleteResult[1].Value.Count, Is.EqualTo(4));
            Assert.That(autocompleteResult[1].Value[0], Is.EqualTo("kanojo"));
            Assert.That(autocompleteResult[1].Value[1], Is.EqualTo("kare"));
            Assert.That(autocompleteResult[1].Value[2], Is.EqualTo("karetachi"));
            Assert.That(autocompleteResult[1].Value[3], Is.EqualTo("kawaii"));

            Assert.That(autocompleteResult[2].Key, Is.EqualTo("kar"));
            Assert.That(autocompleteResult[2].Value.Count, Is.EqualTo(2));
            Assert.That(autocompleteResult[2].Value[0], Is.EqualTo("kare"));
            Assert.That(autocompleteResult[2].Value[1], Is.EqualTo("karetachi"));
        }

        /// <summary>
        /// sample in test task
        /// </summary>
        [Test]
        public void InputTest()
        {
            var frequencyOfWords = new Dictionary<string, int>
            {
                {"kare", 10},    
                {"kanojo", 20},
                {"karetachi", 1},
                {"korosu", 7},
                {"sakura", 3}
            };
            var syllables = new List<string>
            {
                "k",
                "ka",
                "kar"
            };
            var syllableAnalysis = new SyllableAnalysis(frequencyOfWords);
            List<KeyValuePair<string, List<string>>> autocompleteResults = syllableAnalysis.Autocomplete(syllables);

            Assert.That(autocompleteResults.Count, Is.EqualTo(3));

            Assert.That(autocompleteResults[0].Key, Is.EqualTo("k"));
            Assert.That(autocompleteResults[0].Value[0], Is.EqualTo("kanojo"));
            Assert.That(autocompleteResults[0].Value[1], Is.EqualTo("kare"));
            Assert.That(autocompleteResults[0].Value[2], Is.EqualTo("korosu"));
            Assert.That(autocompleteResults[0].Value[3], Is.EqualTo("karetachi"));
            Assert.That(autocompleteResults[0].Value.Count, Is.EqualTo(4));

            Assert.That(autocompleteResults[1].Key, Is.EqualTo("ka"));
            Assert.That(autocompleteResults[1].Value[0], Is.EqualTo("kanojo"));
            Assert.That(autocompleteResults[1].Value[1], Is.EqualTo("kare"));
            Assert.That(autocompleteResults[1].Value[2], Is.EqualTo("karetachi"));
            Assert.That(autocompleteResults[1].Value.Count, Is.EqualTo(3));

            Assert.That(autocompleteResults[2].Key, Is.EqualTo("kar"));
            Assert.That(autocompleteResults[2].Value[0], Is.EqualTo("kare"));
            Assert.That(autocompleteResults[2].Value[1], Is.EqualTo("karetachi"));
            Assert.That(autocompleteResults[2].Value.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void TestEmptyFrequencyOfWords()
        {
            var syllables = new List<string>
            {
                "k",
                "ka",
                "kar"
            };
            var syllableAnalysis = new SyllableAnalysis(new Dictionary<string, int>());
            List<KeyValuePair<string, List<string>>> autocompleteResults = syllableAnalysis.Autocomplete(syllables);
            
            Assert.That(autocompleteResults[0].Key, Is.EqualTo("k"));
            Assert.That(autocompleteResults[1].Key, Is.EqualTo("ka"));
            Assert.That(autocompleteResults[2].Key, Is.EqualTo("kar"));
            Assert.That(autocompleteResults.Count, Is.EqualTo(3));

            Assert.That(autocompleteResults[0].Value.Count, Is.EqualTo(0));
            Assert.That(autocompleteResults[1].Value.Count, Is.EqualTo(0));
            Assert.That(autocompleteResults[2].Value.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestEmptySyllables()
        {
            var frequencyOfWords = new Dictionary<string, int>
            {
               {"kanojo", 20},
                {"karetachi", 1},
                {"korosu", 10},
                {"kare", 10},
                {"sakura", 10}
            };
            var syllableAnalysis = new SyllableAnalysis(frequencyOfWords);
            List<KeyValuePair<string, List<string>>> autocompleteResults = syllableAnalysis.Autocomplete(new List<string>());

            Assert.That(autocompleteResults.Count, Is.EqualTo(0));
        }
    }
}