namespace StringityCore;

using System.IO.Compression;
using System.Security.Cryptography;

/// <summary>
///     Class containing all extension methods defined by the library.
/// </summary>
[PublicAPI]
public static partial class Extensions
{
    /// <summary>
    ///     Gets the total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="string"/>.
    /// </summary>
    /// <param name="input">The string for which <see cref="char"/> elements are to be counted.</param>
    /// <returns>Total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="string"/>.</returns>
    /// <remarks>Total char count may not be equal to number of text elements.</remarks>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountCharacters (this string input) => input.Length;

    /// <summary>
    ///     Gets the total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="ReadOnlySpan{T}"/> of
    ///     <see cref="char"/>.
    /// </summary>
    /// <param name="input">
    ///     The <see cref="ReadOnlySpan{T}"/> of <see cref="char"/> for which <see cref="char"/> elements are to be
    ///     counted.
    /// </param>
    /// <returns>Total count of <see cref="char"/> elements in <paramref name="input"/>.</returns>
    /// <remarks>Total char count may not be equal to number of text elements.</remarks>
    /// <remarks>This method is identical to using <see cref="ReadOnlySpan{T}.Length">ReadOnlySpan&lt;char&gt;.Length</see>.</remarks>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountCharacters (this ReadOnlySpan<char> input) => input.Length;

    /// <summary>
    ///     Gets the total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="string"/> that are in the
    ///     range equivalent to the regular expression [^aeiouAUIOU].
    /// </summary>
    /// <param name="input">The string for which <see cref="char"/> elements are to be counted according to the inclusion rule as stated.</param>
    /// <returns>
    ///     Total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="string"/> that match the inclusion rule
    ///     as stated.
    /// </returns>
    /// <remarks>
    ///     Surrogate pairs, as well as vowels in non-English languages or letters which are functionally vowels in any language,
    ///     including English, are not considered.
    /// </remarks>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountConsonants (this string input)
    {
        return input.Count (static c => char.IsLetter (c) && !"aeiouAEIOU".Contains (c));
    }

    /// <summary>
    ///     Gets the total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="string"/> that are classified
    ///     as Unicode digits.
    /// </summary>
    /// <param name="input">The string for which <see cref="char"/> elements are to be counted according to the inclusion rule as stated.</param>
    /// <returns>
    ///     Total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="string"/> that match the inclusion rule
    ///     as stated.
    /// </returns>
    /// <remarks>Surrogate pairs are not considered.</remarks>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountDigits (this string input) => input.Count (char.IsDigit);

    /// <summary>
    ///     Gets the total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="string"/> that are classified
    ///     as Unicode Lower Case Letters.
    /// </summary>
    /// <param name="input">The string for which <see cref="char"/> elements are to be counted according to the inclusion rule as stated.</param>
    /// <returns>
    ///     Total count of <see cref="char"/> elements in the <paramref name="input"/> <see cref="string"/> that match the inclusion rule
    ///     as stated.
    /// </returns>
    /// <remarks>Surrogate pairs are not considered. All characters must be within the Unicode Basic Multilingual Plane.</remarks>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountLowercase (this string input) => input.Count (char.IsLower);

    /// <summary>
    ///     Gets the total count of paragraphs.<br/>
    ///     A paragraph is counted for every match of this regular expression:
    ///     <code>
    ///  ((\r\n|\r|\n){2,}|\p{Zp}+|\A)^\s*\S
    /// </code>
    ///     This counts all instances of non-whitespace characters following any instances of the following:<br/>
    ///     - The start of the string<br/>
    ///     - Sequences of two or more pairs of carriage return+line feed characters (\u000d\000a)<br/>
    ///     - Sequences of two or more carriage return characters (\u000d)<br/>
    ///     - Sequences of two or more line feed characters (\u000a)<br/>
    ///     - One or more characters that are classified as Unicode Paragraph Separators, excluding the above sequences.<br/>
    ///     Any other whitespace preceding the first non-whitespace character is also ignored and will not affect the count.
    /// </summary>
    /// <param name="input">The string for which paragraphs are to be counted according to the delimiter rules as stated.</param>
    /// <returns>
    ///     Total count of paragraphs according to the delimiter rules as stated.
    /// </returns>
    /// <remarks>
    ///     Single occurrences of carriage return, carriage return+line feed, or line feed are not counted as paragraphs by this method.
    ///     <para>
    ///         See <see href="https://regex101.com/r/uzXE6x">https://regex101.com/r/uzXE6x</see> for detailed explanation and behavior
    ///         demonstration.
    ///     </para>
    /// </remarks>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountParagraphs (this ReadOnlySpan<char> input)
    {
        if (input.IsEmpty || input.IsWhiteSpace ())
        {
            return 0;
        }

        return ParagraphDelimitersRegex ().Count (input);
    }

    /// <inheritdoc cref="CountParagraphs(System.ReadOnlySpan{char})"/>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountParagraphs (this string input) => CountParagraphs (input.AsSpan ());

    /// <summary>
    ///     Gets the total number of char instances in <paramref name="input"/> which are in the Unicode Punctuation class.
    /// </summary>
    /// <param name="input">The text for which punctuation is to be counted.</param>
    /// <returns>The total count of punctuation characters.</returns>
    /// <remarks>
    ///     Surrogate pairs are not considered. All characters must be in the Unicode Basic Multilingual Plane.<br/>
    ///     Pairs of opening and closing punctuation marks are counted as their individual marks - not as pairs.
    /// </remarks>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountPunctuation (this string input) => input.Count (char.IsPunctuation);

    // Count Number of Sentences (Assuming periods, exclamation marks, or question marks indicate a sentence)
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountSentences (this string input) =>
        input.Split (['.', '!', '?'], StringSplitOptions.RemoveEmptyEntries).Length;

    // Count Number of Uppercase Characters
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountUppercase (this string input) => input.Count (char.IsUpper);

    // Count Number of Vowels
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountVowels (this string input) { return input.Count (static c => "aeiouAEIOU".Contains (c)); }

    // Count Number of Whitespace Characters
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountWhitespace (this string input) => input.Count (char.IsWhiteSpace);

    // Count Number of Words
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int CountWords (this string input) =>
        input.Split ([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Length;

    public static string FromBinary (this string input)
    {
        byte [] bytes = input.Split (' ')
                             .Select (static b => Convert.ToByte (b, 2))
                             .ToArray ();

        return Encoding.ASCII.GetString (bytes);
    }

    // Hex Decode
    public static string FromHex (this string input)
    {
        List<byte> chars = [];

        for (int i = 0; i < input.Length; i += 2)
        {
            chars.Add (Convert.ToByte (input.Substring (i, 2), 16));
        }

        return Encoding.UTF8.GetString (chars.ToArray ());
    }

    // Morse Decode
    public static string FromMorseCode (this string input)
    {
        return string.Concat (input.Split (' ').Where (static m => _morseDecode.ContainsKey (m)).Select (static m => _morseDecode [m]));
    }

    // Find Least Frequent Character
    public static string LeastFrequentCharacter (this string input)
    {
        return input.Where (char.IsLetterOrDigit)
                    .GroupBy (static c => c)
                    .OrderBy (static g => g.Count ())
                    .FirstOrDefault ()
                   ?.Key
                    .ToString ()
            ?? string.Empty;
    }

    // Find Least Frequent Word
    public static string LeastFrequentWord (this string input)
    {
        string [] words = input.Split ([' ', '\t', '\n', '\r', '.', ',', '!', '?'], StringSplitOptions.RemoveEmptyEntries);

        return words.GroupBy (static w => w)
                    .OrderBy (static g => g.Count ())
                    .FirstOrDefault ()
                   ?.Key
            ?? string.Empty;
    }

    /// <summary>
    ///     Gets a Unicode-aware count of all printable whitespace and non-whitespace glyphs in <paramref name="input"/>, accounting for
    ///     surrogate pairs and combining marks.<br/>
    ///     This is the logical length of the text, as visible on screen, and may differ significantly from <see cref="string.Length"/>,
    ///     which is a raw count of <see cref="char"/> instances and does not understand surrogate pairs or combining marks.
    /// </summary>
    /// <param name="input">The text to count </param>
    /// <returns>The length of <see cref="input"/> in printable glyphs.</returns>
    /// <remarks>
    ///     Input must be a well-formed string in Unicode Normalization Form C. Most strings will be in this form. However, the caller is
    ///     responsible for ensuring proper normalization before calling this method.<br/>
    ///     This method calls <see cref="StringInfo.ParseCombiningCharacters"/> and returns the length of the result.<br/>
    ///     An array of indexes (<see cref="int"/>[]) is allocated, but no other heap allocations are involved.
    /// </remarks>
    public static int LengthInTextElements (this string input) => StringInfo.ParseCombiningCharacters (input).Length;

    // Find Most Frequent Character
    public static string MostFrequentCharacter (this string input)
    {
        return input.Where (char.IsLetterOrDigit)
                    .GroupBy (static c => c)
                    .OrderByDescending (static g => g.Count ())
                    .FirstOrDefault ()
                   ?.Key
                    .ToString ()
            ?? string.Empty;
    }

    // Find Most Frequent Word
    public static string MostFrequentWord (this string input)
    {
        string [] words = input.Split ([' ', '\t', '\n', '\r', '.', ',', '!', '?'], StringSplitOptions.RemoveEmptyEntries);

        return words.GroupBy (static w => w)
                    .OrderByDescending (static g => g.Count ())
                    .FirstOrDefault ()
                   ?.Key
            ?? string.Empty;
    }

    public static string RemoveDigits (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        return Regex.Replace (input, @"\d", "");
    }

    public static string RemoveLetters (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        return Regex.Replace (input, "[a-zA-Z]", "");
    }

    public static string RemoveNonAlphanumeric (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        return Regex.Replace (input, "[^a-zA-Z0-9]", "");
    }

    public static string RemoveNonAscii (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        return Regex.Replace (input, @"[^\x00-\x7F]", "");
    }

    public static string RemoveSpecialCharacters (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        return Regex.Replace (input, @"[^a-zA-Z0-9\s]", "");
    }

    public static string Shuffle (this string input)
    {
        if (string.IsNullOrEmpty (input))
            return input;

        char [] array = input.ToCharArray ();
        int     n     = array.Length;

        // Fisher-Yates shuffle algorithm
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Shared.Next (i + 1);

            // Swap array[i] with array[j]
            (array [i], array [j]) = (array [j], array [i]);
        }

        return new (array);
    }

    public static string SwapCase (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        char [] array = input.ToCharArray ();

        for (int i = 0; i < array.Length; i++)
        {
            char c = array [i];

            if (char.IsLetter (c))
            {
                array [i] = char.IsUpper (c) ? char.ToLower (c) : char.ToUpper (c);
            }
        }

        return new (array);
    }

    public static string ToAscii (this string input) => Encoding.ASCII.GetString (Encoding.ASCII.GetBytes (input));

    public static string ToBinary (this string input)
    {
        return string.Join (" ", input.Select (static c => Convert.ToString (c, 2).PadLeft (8, '0')));
    }

    public static string ToCamelCase (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        string [] words = Regex.Split (input, @"[\s_\-]+");

        if (words.Length == 0)
            return input;

        for (int i = 1; i < words.Length; i++)
        {
            words [i] = char.ToUpper (words [i] [0]) + words [i].Substring (1).ToLower ();
        }

        return words [0].ToLower () + string.Join ("", words.Skip (1));
    }

    public static string ToCompress (this string input)
    {
        byte [] bytes = Encoding.Unicode.GetBytes (input);

        using MemoryStream msi = new (bytes);
        using MemoryStream mso = new ();

        using (GZipStream gs = new (mso, CompressionMode.Compress))
        {
            msi.CopyTo (gs);
        }

        return Convert.ToBase64String (mso.ToArray ());
    }

    public static string ToDecompress (this string input)
    {
        byte [] bytes = Convert.FromBase64String (input);

        using MemoryStream msi = new (bytes);
        using MemoryStream mso = new ();

        using (GZipStream gs = new (msi, CompressionMode.Decompress))
        {
            gs.CopyTo (mso);
        }

        return Encoding.Unicode.GetString (mso.ToArray ());
    }

    public static string ToHex (this string input) { return string.Concat (input.Select (static c => ((int)c).ToString ("X2"))); }

    public static string ToJsonEscaped (this string input)
    {
        if (string.IsNullOrEmpty (input))
            return input;

        StringBuilder sb = new ();

        foreach (char c in input)
        {
            switch (c)
            {
                case '\"':
                    sb.Append ("\\\"");

                    break;
                case '\\':
                    sb.Append ("\\\\");

                    break;
                case '\b':
                    sb.Append ("\\b");

                    break;
                case '\f':
                    sb.Append ("\\f");

                    break;
                case '\n':
                    sb.Append ("\\n");

                    break;
                case '\r':
                    sb.Append ("\\r");

                    break;
                case '\t':
                    sb.Append ("\\t");

                    break;
                default:
                    if (c < ' ' || c > 127)
                        sb.Append ($"\\u{(int)c:X4}");
                    else
                        sb.Append (c);

                    break;
            }
        }

        return sb.ToString ();
    }

    public static string ToKebabCase (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        return Regex.Replace (input, "([a-z])([A-Z])", "$1-$2")
                    .Replace ("_", "-")
                    .Replace (" ", "-")
                    .ToLower ();
    }

    public static string ToMorseCode (this string input)
    {
        return string.Join (" ", input.ToUpper ().Where (static c => _morseAlphabet.ContainsKey (c)).Select (static c => _morseAlphabet [c]));
    }

    public static string ToPascalCase (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        string [] words = Regex.Split (input, @"[\s_\-]+");

        if (words.Length == 0)
            return input;

        for (int i = 0; i < words.Length; i++)
        {
            words [i] = char.ToUpper (words [i] [0]) + words [i].Substring (1).ToLower ();
        }

        return string.Join ("", words);
    }

    // ROT13 Encode/Decode
    public static string ToRot13 (this string input) => string.Concat (input.Select (Rot13Char));

    /// <summary>
    ///     Returns a string in aLtErNaTiNg CaSe, as commonly used to indicate sarcasm in internet communication.
    /// </summary>
    /// <param name="input">The boring string to make sArCaStIc.</param>
    /// <returns>A new string with the same content as <paramref name="input"/>, but with aLtErNaTiNg CaSe.</returns>
    public static string ToSarcasm (this string input)
    {
        if (string.IsNullOrEmpty (input))
            return input;

        char [] result = new char[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            // Check if the position is odd or even (0-indexed)
            if (i % 2 == 0)
            {
                // Even index: make it lowercase
                result [i] = char.ToLower (input [i], CultureInfo.CurrentUICulture);
            }
            else
            {
                // Odd index: make it uppercase
                result [i] = char.ToUpper (input [i], CultureInfo.CurrentUICulture);
            }
        }

        return new (result);
    }

    public static string ToSHA256 (this string input)
    {
        byte [] bytes = Encoding.UTF8.GetBytes (input);
        byte [] hash  = SHA256.Create ().ComputeHash (bytes);

        return Convert.ToHexString (hash).ToLower ();
    }

    public static string ToSnakeCase (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        return Regex.Replace (input, "([a-z])([A-Z])", "$1_$2")
                    .Replace ("-", "_")
                    .Replace (" ", "_")
                    .ToLower ();
    }

    public static string ToStringReverse (this string input)
    {
        string strReversed = new (input.Reverse ().ToArray ());

        return strReversed;
    }

    public static string ToTitleCase (this string input)
    {
        if (string.IsNullOrWhiteSpace (input))
            return input;

        string [] words = Regex.Split (input, @"[\s_\-]+");

        if (words.Length == 0)
            return input;

        for (int i = 0; i < words.Length; i++)
        {
            words [i] = char.ToUpper (words [i] [0]) + words [i].Substring (1).ToLower ();
        }

        return string.Join (" ", words);
    }

    /// <summary>
    ///     Gets a new string that is the same as <paramref name="input"/>, converted to UTF-16 little-endian.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>A new string that is the same as <paramref name="input"/>, converted to UTF-16 little-endian.</returns>
    /// <remarks>
    ///     Most Unicode implementations expect UTF-16 to be little-endian, unless a byte order mark is included and understood by the
    ///     recipient.
    /// </remarks>
    /// <remarks>This method will not include a byte order mark.</remarks>
    public static string ToUnicode (this string input) => Encoding.Unicode.GetString (Encoding.Unicode.GetBytes (input));

    /// <summary>
    ///     Gets a new string that is the same as <paramref name="input"/>, converted to UTF-16 big-endian.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>A new string that is the same as <paramref name="input"/>, converted to UTF-16 big-endian.</returns>
    /// <remarks>
    ///     Most Unicode implementations expect UTF-16 to be little-endian, unless a byte order mark is included and understood by the
    ///     recipient.
    /// </remarks>
    /// <remarks>This method will not include a byte order mark.</remarks>
    public static string ToUtf16BE (this string input) => Encoding.BigEndianUnicode.GetString (Encoding.BigEndianUnicode.GetBytes (input));

    /// <summary>
    ///     Gets a new string that is the same as <paramref name="input"/>, converted to UTF-32 little-endian.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>A new string that is the same as <paramref name="input"/>, converted to UTF-32 little-endian.</returns>
    /// <remarks>
    ///     Most Unicode implementations expect UTF-32 to be little-endian, unless a byte order mark is included and understood by the
    ///     recipient.
    /// </remarks>
    /// <remarks>This method will not include a byte order mark.</remarks>
    public static string ToUtf32 (this string input) => Encoding.UTF32.GetString (Encoding.UTF32.GetBytes (input));

    // Convert to UTF-8
    public static string ToUtf8 (this string input) => Encoding.UTF8.GetString (Encoding.UTF8.GetBytes (input));

    public static string ToXmlEscaped (this string input)
    {
        if (string.IsNullOrEmpty (input))
            return input;

        return input.Replace ("&", "&amp;")
                    .Replace ("\"", "&quot;")
                    .Replace ("'",  "&apos;")
                    .Replace ("<",  "&lt;")
                    .Replace (">",  "&gt;");
    }

    /// <summary>
    ///     Regex that defines what is considered a paragraph separator.
    /// </summary>
    [GeneratedRegex (
                        "((\\r\\n|\\r|\\n|\\p{Zp}+){2,}|\\A)^\\s*\b\\S",
                        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking | RegexOptions.Multiline
                    )]
    internal static partial Regex ParagraphDelimitersRegex ();

    private static char Rot13Char (char input)
    {
        return input switch
               {
                   >= 'a' and <= 'z' => (char)('a' + (input - 'a' + 13) % 26),
                   >= 'A' and <= 'Z' => (char)('A' + (input - 'A' + 13) % 26),
                   _                 => input
               };
    }

    #region Private tools

    private static readonly Dictionary<char, string> _morseAlphabet = new ()
                                                                      {
                                                                          { 'A', ".-" }, { 'B', "-..." }, { 'C', "-.-." }, { 'D', "-.." },
                                                                          { 'E', "." },
                                                                          { 'F', "..-." }, { 'G', "--." }, { 'H', "...." }, { 'I', ".." },
                                                                          { 'J', ".---" },
                                                                          { 'K', "-.-" }, { 'L', ".-.." }, { 'M', "--" }, { 'N', "-." },
                                                                          { 'O', "---" },
                                                                          { 'P', ".--." }, { 'Q', "--.-" }, { 'R', ".-." }, { 'S', "..." },
                                                                          { 'T', "-" },
                                                                          { 'U', "..-" }, { 'V', "...-" }, { 'W', ".--" }, { 'X', "-..-" },
                                                                          { 'Y', "-.--" },
                                                                          { 'Z', "--.." }, { '1', ".----" }, { '2', "..---" }, { '3', "...--" },
                                                                          { '4', "....-" },
                                                                          { '5', "....." }, { '6', "-...." }, { '7', "--..." }, { '8', "---.." },
                                                                          { '9', "----." },
                                                                          { '0', "-----" }
                                                                      };

    private static readonly Dictionary<string, char> _morseDecode = _morseAlphabet.ToDictionary (static kvp => kvp.Value, static kvp => kvp.Key);

    #endregion
}
