using CsvHelper.Configuration.Attributes;

namespace Parth_Traders.CsvParserModel
{
    public class ParsedSupplier
    {
        [Index(0)]
        public string SupplierName { get; set; }

        [Index(1)]
        public string SupplierAddress { get; set; }

        [Index(2)]
        public string SupplierEmail { get; set; }

        [Index(3)]
        public string SupplierPhoneNumber { get; set; }
    }
}
