
using CsvHelper.Configuration.Attributes;

namespace Parth_Traders.CsvParserModel
{
    //TODO: 1.Admin API
    public class ParsedCustomer
    {
        [Index(0)]
        public string CustomerName { get; set; }

        [Index(1)]
        public DateTime CreatedDate { get; set; }

        [Index(2)]
        public string CustomerEmail { get; set; }

        [Index(3)]
        public string CustomerPhoneNumber { get; set; }

        [Index(4)]
        public string CustomerAddress { get; set; }

        [Index(5)]
        public int PaymentType { get; set; }
    }
}
