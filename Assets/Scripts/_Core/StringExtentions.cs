using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace _Core
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

        public static bool IsFilenameValid(this string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return false;

            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return false;

            if (filename.EndsWith(' ') || filename.EndsWith('.'))
                return false;

            string[] reserved =
            {
                "CON", "PRN", "AUX", "NUL",
                "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
            };

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(filename);

            return !reserved.Contains(nameWithoutExtension, StringComparer.OrdinalIgnoreCase);
        }

        public static string FramesToSeconds(this int frames)
        {
            float secondsLeft = Time.fixedDeltaTime * frames;
            return secondsLeft.ToString("F1");
        }
    }

    public enum ZeroInsertionMode
    {
        ToTheLeft,
        ToTheRight
    }

}