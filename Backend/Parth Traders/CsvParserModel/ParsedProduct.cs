using CsvHelper.Configuration.Attributes;
using Parth_Traders.Domain.Enums;

namespace Parth_Traders.CsvParserModel
{
    public class ParsedProduct
    {
        [Index(0)]
        public string ProductName { get; set; }

        [Index(1)]
        public ProductType ProductType { get; set; }

        [Index(2)]
        public string ProductDescription { get; set; }

        [Index(3)]
        public string SupplierName { get; set; }

        [Index(4)]
        public string CategoryName { get; set; }

        [Index(5)]
        public int PiecesPerUnit { get; set; }

        [Index(6)]
        public long UnitPrice { get; set; }

        [Index(7)]
        public long SinglePieceMRP { get; set; }

        [Index(8)]
        public long Discount { get; set; }

        [Index(9)]
        public long UnitsInStock { get; set; }

    }
}
