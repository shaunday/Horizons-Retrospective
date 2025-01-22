namespace HsR.Common.ContentGenerators
{
    public class RandomNumberGenerator
    {
        private readonly Random _random = new();

        public string GenerateRandomNumber(int length)
        {
            if (length <= 0) throw new ArgumentException("Length must be greater than zero.", nameof(length));

            var number = new char[length];

            // Ensure the first digit is non-zero to avoid leading zeros
            number[0] = (char)('1' + _random.Next(0, 9));

            for (int i = 1; i < length; i++)
            {
                number[i] = (char)('0' + _random.Next(0, 10));
            }

            return new string(number);
        }
    }
}
