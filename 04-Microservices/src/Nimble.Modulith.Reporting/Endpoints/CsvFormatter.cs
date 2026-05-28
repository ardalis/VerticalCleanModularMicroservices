using System.Globalization;
using System.Text;

namespace Nimble.Modulith.Reporting.Endpoints;

/// <summary>
/// Utility to convert report data to CSV format
/// </summary>
public static class CsvFormatter
{
    public static string ToCsv<T>(IEnumerable<T> records)
    {
        var sb = new StringBuilder();
        var properties = typeof(T).GetProperties();

        // Header
        sb.AppendLine(string.Join(",", properties.Select(p => EscapeCsvField(p.Name))));

        // Rows
        foreach (var record in records)
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(record);
                return EscapeCsvField(value?.ToString() ?? "");
            });
            sb.AppendLine(string.Join(",", values));
        }

        return sb.ToString();
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}
