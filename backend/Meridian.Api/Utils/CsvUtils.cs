using System.Text;

namespace Meridian.Api.Utils
{
    public static class CsvUtils
    {
        // Splits a CSV line, ignoring commas inside quotes
        public static List<string> SplitCsvLine(string line)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            var inQuotes = false;

            foreach (var c in line)
            {
                if (c == '"') inQuotes = !inQuotes;
                else if (c == ',' && !inQuotes) { result.Add(current.ToString()); current.Clear(); }
                else current.Append(c);
            }
            result.Add(current.ToString());
            return result;
        }

        // Returns the error message, or null if the row is valid
        public static string? CheckRow(List<string> columns, List<string> countryCodes)
        {
            if (columns.Count < 7)
                return "Malformed line";
            if (!countryCodes.Contains(columns[4]))
                return $"Unknown country code '{columns[4]}'";

            if (columns[6] != "Y" && columns[6] != "N" && columns[6] != "A" && columns[6] != "X")
                return $"Unknown ballot value '{columns[6]}'";

            return null;
        }

    }
}
