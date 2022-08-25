
using CsvHelper.Configuration.Attributes;
using Parth_Traders.Domain.Enums;

namespace Parth_Traders.CsvParserModel
{
    //TODO: 1.Admin API
    public class ParsedCustomer
    {
        [Index(0)]
        public string CustomerName { get; set; }

        [Index(1)]
        public string CustomerEmail { get; set; }

        [Index(2)]
        public string CustomerPhoneNumber { get; set; }

        [Index(3)]
        public string CustomerAddress { get; set; }

        [Index(4)]
        public PaymentType PaymentType { get; set; }
    }
}
