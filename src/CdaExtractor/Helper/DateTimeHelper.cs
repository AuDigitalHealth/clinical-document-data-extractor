using System;
using System.Globalization;

namespace Nehta.VendorLibrary.CdaExtractor.Helper
{
    class DateTimeHelper
    {
        private const string FormatTemplate = "yyyyMMddHHmmss.ffff";

        public static DateTime? ConvertISO8601DateTimeStringToDateTime(string timestring)
        {
            if (timestring == null)
            {
                return null;
            }

            try
            {
                return ConvertUtcTimeStringToDateTime(GetUtcTimeFromIso(timestring));
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static DateTime ConvertUtcTimeStringToDateTime(string timestring)
        {
            return DateTime.ParseExact(timestring, FormatTemplate.Substring(0, timestring.Length), null);
        }

        private static string GetUtcTimeFromIso(string timestring)
        {
            var plusMinusIndex = timestring.IndexOf("+", StringComparison.Ordinal);
            if (plusMinusIndex < 0) plusMinusIndex = timestring.IndexOf("-", StringComparison.Ordinal);

            if (plusMinusIndex >= 10)
            {
                var format = FormatTemplate.Substring(0, plusMinusIndex);
                var timezoneLength = timestring.Substring(plusMinusIndex + 1).Length;

                var timezoneFormat = "zzzz".Substring(0, timezoneLength);
                var equivalent = DateTime.ParseExact(timestring, format + timezoneFormat, CultureInfo.InvariantCulture);

                // Get output format
                var outputFormat = format;
                if (timezoneLength == 4 && !timestring.EndsWith("00") && format.Length < 12)
                    outputFormat = FormatTemplate.Substring(0, 12);
                if (outputFormat.Length > 14)
                    outputFormat = outputFormat.Substring(0, 14);

                return equivalent.ToUniversalTime().ToString(outputFormat);
            }
            if (plusMinusIndex < 10 && plusMinusIndex > -1)
            {
                return timestring.Substring(0, plusMinusIndex);
            }
            return timestring.Length > 14 ? timestring.Substring(0, 14) : timestring;
        }

    }
}
