using System;
namespace MBD.Core.Extensions
{
    public static class StringExtensions
    {
        public static string[] ConvertToArray(this string text) =>
            text.Split(Environment.NewLine);
    }
}