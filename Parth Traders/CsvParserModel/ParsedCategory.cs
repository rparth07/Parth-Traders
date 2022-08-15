using CsvHelper.Configuration.Attributes;

namespace Parth_Traders.CsvParserModel
{
    public class ParsedCategory
    {
        [Index(0)]
        public string CategoryName { get; set; }

        [Index(1)]
        public string CategoryDescription { get; set; }
    }
}
