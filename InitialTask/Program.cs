using System;
using System.Collections.Generic;
using Prompter;

namespace InitialTask
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                InputDataController inputController;
                SyllableAnalysis syllableInstance;
                if (args.Length == 1)
                {
                    inputController = new InputDataController();
                    inputController.PerformAutomaticInput(args[0]);
                    syllableInstance = new SyllableAnalysis(inputController.FrequencyOfWords);
                    foreach (var syllable in inputController.Syllables)
                    {
                        var autocompleteResult = syllableInstance.Autocomplete(syllable);
                        Console.WriteLine("{0}:", syllable);
                        foreach (var resultWord in autocompleteResult)
                        {
                            Console.WriteLine(resultWord);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("\n\n");
                    Console.WriteLine("Elapsed time of the job: {0} ms", syllableInstance.DurationOfTime.TotalMilliseconds);
                }
                else
                {
                    inputController = new InputDataController();
                    inputController.PerformUserInput();
                    syllableInstance = new SyllableAnalysis(inputController.FrequencyOfWords);
                    List<KeyValuePair<string, List<string>>> autocompleteResults = syllableInstance.Autocomplete(inputController.Syllables);
                    Console.WriteLine();
                    foreach (var autocompleteResult in autocompleteResults)
                    {
                        foreach (var autocompleteWord in autocompleteResult.Value)
                        {
                            Console.WriteLine(autocompleteWord);
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}