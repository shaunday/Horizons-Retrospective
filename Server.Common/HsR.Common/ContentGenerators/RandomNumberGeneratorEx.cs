using System;
using System.Security.Cryptography;

namespace HsR.Common.ContentGenerators
{
    public class RandomNumberGeneratorEx
    {
        public string GenerateRandomNumber(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than zero.", nameof(length));

            char[] number = new char[length];

            for (int i = 0; i < length; i++)
            {
                // Use RandomNumberGenerator.GetInt32 to generate a random integer between 0 and 9
                int randomDigit = RandomNumberGenerator.GetInt32(0, 10);
                number[i] = (char)('0' + randomDigit);
            }

            return new string(number);
        }
    }
}
