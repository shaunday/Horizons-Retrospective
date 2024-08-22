using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayJTrading.Journal.Seeder
{
    internal class RandomWordsGenerator
    {
        private readonly Random _random = new Random();
        private const string _vowels = "aeiou";
        private const string _consonants = "bcdfghjklmnpqrstvwxyz";

        public string GenerateRandomWord(int length)
        {
            if (length <= 0) throw new ArgumentException("Length must be greater than zero.", nameof(length));

            var word = new StringBuilder(length);

            // Alternate between consonants and vowels
            bool useVowel = _random.Next(0, 2) == 0;
            for (int i = 0; i < length; i++)
            {
                if (useVowel)
                {
                    word.Append(_vowels[_random.Next(_vowels.Length)]);
                }
                else
                {
                    word.Append(_consonants[_random.Next(_consonants.Length)]);
                }

                useVowel = !useVowel; // Alternate for the next character
            }

            return word.ToString();
        }
    }
}
