using HtmlAgilityPack;

namespace MyBCA.Server.Services.Bus;

static class BusSheetReader
{
    public static Dictionary<string, string> ParseTableToPositionMap(HtmlDocument doc)
    {
        // In Google Sheets, main sheet table is located in table element with waffle class
        var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'waffle')]")
            ?? throw new InvalidDataException("Table not found on page");

        // Skip first row (header)
        var rows = table.SelectNodes("tbody/tr").Cast<HtmlNode>().Skip(1);
        var positionMap = new Dictionary<string, string>();

        foreach (var row in rows)
        {
            var cells = row.SelectNodes("td").Cast<HtmlNode>();
            for (int i = 0; i < 4; i += 2)
            {
                var cellContent = cells.ElementAt(i).InnerText;
                if (string.IsNullOrWhiteSpace(cellContent))
                {
                    continue;
                }

                positionMap[cellContent] = cells.ElementAt(i + 1).InnerText;
            }
        }

        return positionMap;
    }
}