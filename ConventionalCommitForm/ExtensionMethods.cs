using System;
using System.Collections.Generic;
using System.Text;

namespace ConventionalCommitForm
{
    public static class ExtensionMethods
    {
        public static int CountWidth(this string value)
        {
            var max = 0;
            var line = 0;

            foreach (var c in value)
            {
                if (IsNewLine(c))
                {
                    max = Math.Max(line, max);
                    line = 0;
                }
                else if (IsTab(c))
                {
                    line += 4;
                }
                else
                {
                    line++;
                }
            }

            return Math.Max(line, max);

            static bool IsTab(char c) =>
                c == '\t';

            static bool IsNewLine(char c) =>
                c == '\r' ||
                c == '\n';
        }

        public static void AppendOptional(this StringBuilder builder, string prefix, string content, string suffix)
        {
            if (string.IsNullOrEmpty(content))
                return ;

            builder.Append(prefix);
            builder.Append(content);
            builder.Append(suffix);
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            foreach(var value in values)
            {
                collection.Add(value);
            }
        }
    }
}