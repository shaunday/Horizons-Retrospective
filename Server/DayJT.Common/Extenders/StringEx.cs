namespace DayJTrading.Common.Extenders
{
    public static class StringEx
    {
        public static bool IsValidInteger(this string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
