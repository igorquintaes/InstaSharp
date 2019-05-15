using System;
using System.Linq;

namespace InstaSharp.Shared.Extensions
{
    public static class StringExtensions
    {
        public static int OnlyNumbers(this string text) =>
            Convert.ToInt32(string.Concat(text.Where(x => char.IsNumber(x))));
    }
}
