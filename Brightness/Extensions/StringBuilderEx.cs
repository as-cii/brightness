using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Extensions
{
    public static class StringBuilderEx
    {
        public static void AppendLineIfNonEmpty(this StringBuilder builder, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            builder.AppendLine(text);
        }
    }
}
