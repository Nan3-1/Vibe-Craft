namespace VibeCraft.Web.Helpers
{
    public static class CodeGenerator
    {
        // Генерира код за събитие
        public static string GenerateEventCode(string eventType)
        {
            var prefix = eventType switch
            {
                "Wedding" => "WED",
                "CorporateMeeting" => "CORP",
                "BirthdayParty" => "BDAY",
                "ConcertFestival" => "CONC",
                _ => "EVT"
            };
            
            var random = new Random();
            var randomCode = new string(Enumerable
                .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
                
            return $"{prefix}-{randomCode}-{DateTime.UtcNow.Year}";
        }
    }
}