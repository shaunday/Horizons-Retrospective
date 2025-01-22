namespace HsR.Common.ContentGenerators
{
    public class RandomWordGenerator
    {
        private readonly Random _random = new();
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public string GenerateRandomWord(int length)
        {
            if (length <= 0) throw new ArgumentException("Length must be greater than zero.", nameof(length));

            char[] word = new char[length];

            for (int i = 0; i < length; i++)
            {
                int randomIndex = _random.Next(alphabet.Length);
                word[i] = alphabet[randomIndex];
            }

            return new string(word);
        }
    }
}
