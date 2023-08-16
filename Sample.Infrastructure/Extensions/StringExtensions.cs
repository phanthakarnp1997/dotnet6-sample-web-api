using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (char.IsUpper(c))
                {
                    if (i > 0 && char.IsLetterOrDigit(input[i - 1]))
                        sb.Append('_');

                    sb.Append(char.ToUpper(c));
                }
                else
                {
                    sb.Append(char.ToUpper(c));
                }
            }

            return sb.ToString();
        }
    }
}
