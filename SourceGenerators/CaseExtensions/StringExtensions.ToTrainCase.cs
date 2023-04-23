using System;

namespace CaseExtensions
{
    public static partial class StringExtensions
    {
        public static string ToTrainCase(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return SymbolsPipe(
                source,
                '-',
                (s, disableFrontDelimeter) =>
                {
                    if (disableFrontDelimeter)
                    {
                        return new char[] { char.ToUpperInvariant(s) };
                    }

                    return new char[] { '-', char.ToUpperInvariant(s) };
                });
        }
    }
}
