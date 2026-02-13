namespace VibeCraft.Web.Helpers
{
    public static class TextParser
    {
        // Парсва "виб" стринга в списък
        public static List<string> ParseVibeString(string vibeString)
        {
            if (string.IsNullOrWhiteSpace(vibeString))
                return new List<string>();

            return vibeString.Split(',', ';', '|')
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();
        }
    }
}