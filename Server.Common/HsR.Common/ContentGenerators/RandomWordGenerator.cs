using System;
using System.Security.Cryptography;

namespace HsR.Common.ContentGenerators
{
    public class RandomWordGenerator
    {
        const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        public string GenerateRandomWord(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than zero.", nameof(length));

            char[] word = new char[length];
            byte[] randomBytes = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes); // Fill the array with cryptographically secure random bytes
            }

            for (int i = 0; i < length; i++)
            {
                int randomIndex = randomBytes[i] % Alphabet.Length; // Map the byte value to the alphabet range
                word[i] = Alphabet[randomIndex];
            }

            return new string(word);
        }
    }
}
