﻿using CsvHelper;
using System.Globalization;

namespace Parth_Traders.Helper
{
    public static class DataParsingHelper
    {
        public static List<T> ParseData<T>(this T dataObject, IFormFile postedFile)
        {
            try
            {
                List<T> parsedData = new List<T>();

                using (StreamReader streamReader = new StreamReader(postedFile.OpenReadStream()))
                {
                    var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);
                    parsedData = csvReader.GetRecords<T>().ToList();
                }

                return parsedData;
            }

            catch (Exception ex)
            {
                throw new Exception("Can not proccess File Please check and Try again.");
            }

        }
    }
}
