using System;

namespace Extentions
{
    public static class StringExtentions
    {
        public static string SecondsToFormatTime(this int seconds, bool showHours)
        {
            string result = "";

            if (showHours)
                result += $"{seconds / 3600}:";
            result += $"{((seconds % 3600) / 60).InsertZerosToFillLength(2, ZeroInsertionMode.ToTheLeft)}:";
            result += $"{(seconds % 60).InsertZerosToFillLength(2, ZeroInsertionMode.ToTheLeft)}";
            
            return result;
        }

        public static string InsertZerosToFillLength(this int number, int length, ZeroInsertionMode insertionMode)
        {
            string result = number.ToString();
            while (result.Length < length)
            {
                result = insertionMode switch
                {
                    ZeroInsertionMode.ToTheLeft => "0" + result,
                    ZeroInsertionMode.ToTheRight => result + "0",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            return result;
        }
    }

    public enum ZeroInsertionMode
    {
        ToTheLeft,
        ToTheRight
    }

}