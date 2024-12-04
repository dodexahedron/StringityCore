using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace StringityCore;

/// <summary>
///     Class containing all extension methods defined by the library.
/// </summary>
[UsedImplicitly (ImplicitUseTargetFlags.Itself)]
public static class Extensions
{
    /// <summary>
    ///     Returns a string in aLtErNaTiNg CaSe, as commonly used to indicate sarcasm in internet communication.
    /// </summary>
    /// <param name="input">The boring string to make sArCaStIc.</param>
    /// <returns>A new string with the same content as <paramref name="input"/>, but with aLtErNaTiNg CaSe.</returns>
    public static string ToSarcasm(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        char[] result = new char[input.Length];

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

    public static string ToStringReverse(this string input)
    {
        string strReversed = new (input.Reverse().ToArray());

        return strReversed;
    }

    public static string ToSHA256(this string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hash  = SHA256.Create().ComputeHash(bytes);

        return BytesToHex(hash);
    }

    public static string ToCompress(this string input)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(input);

        using MemoryStream msi = new (bytes);
        using MemoryStream mso = new ();

        using (GZipStream gs = new (mso, CompressionMode.Compress))
        {
            msi.CopyTo(gs);
        }

        return Convert.ToBase64String(mso.ToArray());
    }

    public static string ToDecompress(this string input)
    {
        byte[] bytes = Convert.FromBase64String(input);

        using MemoryStream msi = new (bytes);
        using MemoryStream mso = new ();

        using (GZipStream gs = new (msi, CompressionMode.Decompress))
        {
            gs.CopyTo(mso);
        }

        return Encoding.Unicode.GetString(mso.ToArray());
    }

    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return System.Text.RegularExpressions.Regex.Replace(input, @"([a-z])([A-Z])", "$1_$2")
                     .Replace("-", "_")
                     .Replace(" ", "_")
                     .ToLower();
    }

    public static string ToKebabCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return System.Text.RegularExpressions.Regex.Replace(input, @"([a-z])([A-Z])", "$1-$2")
                     .Replace("_", "-")
                     .Replace(" ", "-")
                     .ToLower();
    }

    public static string ToCamelCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        string[] words = System.Text.RegularExpressions.Regex.Split(input, @"[\s_\-]+");

        if (words.Length == 0)
            return input;

        for (int i = 1; i < words.Length; i++)
        {
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
        }

        return words[0].ToLower() + string.Join("", words.Skip(1));
    }

    public static string ToPascalCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        string[] words = System.Text.RegularExpressions.Regex.Split(input, @"[\s_\-]+");

        if (words.Length == 0)
            return input;

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
        }

        return string.Join("", words);
    }

    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        string[] words = System.Text.RegularExpressions.Regex.Split(input, @"[\s_\-]+");

        if (words.Length == 0)
            return input;

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
        }

        return string.Join(" ", words);
    }

    public static string RemoveNonAlphanumeric(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return System.Text.RegularExpressions.Regex.Replace(input, @"[^a-zA-Z0-9]", "");
    }

    public static string RemoveNonAscii(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return System.Text.RegularExpressions.Regex.Replace(input, @"[^\x00-\x7F]", "");
    }

    public static string RemoveDigits(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return System.Text.RegularExpressions.Regex.Replace(input, @"\d", "");
    }

    public static string RemoveLetters(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return System.Text.RegularExpressions.Regex.Replace(input, @"[a-zA-Z]", "");
    }

    public static string RemoveSpecialCharacters(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return System.Text.RegularExpressions.Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");
    }

    public static string Shuffle(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        char[] array = input.ToCharArray();
        int    n     = array.Length;

        // Fisher-Yates shuffle algorithm
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);

            // Swap array[i] with array[j]
            char temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        return new (array);
    }

    public static string SwapCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        char[] array = input.ToCharArray();

        for (int i = 0; i < array.Length; i++)
        {
            char c = array[i];

            if (char.IsLetter(c))
            {
                array[i] = char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c);
            }
        }

        return new (array);
    }

    public static string ToHex(this string input)
    {
        return string.Concat(input.Select(c => ((int)c).ToString("X2")));
    }

    // Hex Decode
    public static string FromHex(this string input)
    {
        List<byte> chars = new ();

        for (int i = 0; i < input.Length; i += 2)
        {
            chars.Add(Convert.ToByte(input.Substring(i, 2), 16));
        }

        return Encoding.UTF8.GetString(chars.ToArray());
    }

    public static string ToAscii(this string input)
    {
        return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(input));
    }

    // Convert to Unicode
    public static string ToUnicode(this string input)
    {
        return Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(input));
    }

    // Convert to UTF-8
    public static string ToUtf8(this string input)
    {
        return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input));
    }

    // Convert to UTF-16
    public static string ToUtf16(this string input)
    {
        return Encoding.BigEndianUnicode.GetString(Encoding.BigEndianUnicode.GetBytes(input));
    }

    // Convert to UTF-32
    public static string ToUtf32(this string input)
    {
        return Encoding.UTF32.GetString(Encoding.UTF32.GetBytes(input));
    }

    public static string ToMorseCode(this string input)
    {
        return string.Join(" ", input.ToUpper().Where(c => MorseAlphabet.ContainsKey(c)).Select(c => MorseAlphabet[c]));
    }

    // Morse Decode
    public static string FromMorseCode(this string input)
    {
        return string.Concat(input.Split(' ').Where(m => MorseDecode.ContainsKey(m)).Select(m => MorseDecode[m]));
    }

    // ROT13 Encode/Decode
    public static string ToRot13(this string input)
    {
        return string.Concat(input.Select(c => Rot13Char(c)));
    }

    private static char Rot13Char(char input)
    {
        if (input >= 'a' && input <= 'z')
        {
            return (char)('a' + (input - 'a' + 13) % 26);
        }
        else if (input >= 'A' && input <= 'Z')
        {
            return (char)('A' + (input - 'A' + 13) % 26);
        }
        else
        {
            return input;
        }
    }

    public static string ToBinary(this string input)
    {
        return string.Join(" ", input.Select(c => Convert.ToString(c, 2).PadLeft(8, '0')));
    }

    public static string FromBinary(this string input)
    {
        byte[] bytes = input.Split(' ')
                            .Select(b => Convert.ToByte(b, 2))
                            .ToArray();

        return Encoding.ASCII.GetString(bytes);
    }

    // Get String Length
    public static string GetLength(this string input)
    {
        return input.Length.ToString();
    }

    // Count Number of Words
    public static string CountWords(this string input)
    {
        return input.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length.ToString();
    }

    // Count Number of Characters
    public static string CountCharacters(this string input)
    {
        return input.Length.ToString();
    }

    // Count Number of Vowels
    public static string CountVowels(this string input)
    {
        return input.Count(c => "aeiouAEIOU".Contains(c)).ToString();
    }

    // Count Number of Consonants
    public static string CountConsonants(this string input)
    {
        return input.Count(c => char.IsLetter(c) && !"aeiouAEIOU".Contains(c)).ToString();
    }

    // Count Number of Digits
    public static string CountDigits(this string input)
    {
        return input.Count(char.IsDigit).ToString();
    }

    // Count Number of Uppercase Characters
    public static string CountUppercase(this string input)
    {
        return input.Count(char.IsUpper).ToString();
    }

    // Count Number of Lowercase Characters
    public static string CountLowercase(this string input)
    {
        return input.Count(char.IsLower).ToString();
    }

    // Count Number of Whitespace Characters
    public static string CountWhitespace(this string input)
    {
        return input.Count(char.IsWhiteSpace).ToString();
    }

    // Count Number of Punctuation Marks
    public static string CountPunctuation(this string input)
    {
        return input.Count(char.IsPunctuation).ToString();
    }

    // Count Number of Sentences (Assuming periods, exclamation marks, or question marks indicate a sentence)
    public static string CountSentences(this string input)
    {
        return input.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length.ToString();
    }

    // Count Number of Paragraphs (Assuming double newline is a paragraph separator)
    public static string CountParagraphs(this string input)
    {
        return input.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length.ToString();
    }

    // Find Most Frequent Character
    public static string MostFrequentCharacter(this string input)
    {
        return input.Where(char.IsLetterOrDigit)
                    .GroupBy(c => c)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key.ToString() ?? string.Empty;
    }

    // Find Most Frequent Word
    public static string MostFrequentWord(this string input)
    {
        string[] words = input.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        return words.GroupBy(w => w)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key ?? string.Empty;
    }

    // Find Least Frequent Character
    public static string LeastFrequentCharacter(this string input)
    {
        return input.Where(char.IsLetterOrDigit)
                    .GroupBy(c => c)
                    .OrderBy(g => g.Count())
                    .FirstOrDefault()?.Key.ToString() ?? string.Empty;
    }

    // Find Least Frequent Word
    public static string LeastFrequentWord(this string input)
    {
        string[] words = input.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        return words.GroupBy(w => w)
                    .OrderBy(g => g.Count())
                    .FirstOrDefault()?.Key ?? string.Empty;
    }

    public static string ToJsonEscaped(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        StringBuilder sb = new ();

        foreach (char c in input)
        {
            switch (c)
            {
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                default:
                    if (c < ' ' || c > 127)
                        sb.AppendFormat("\\u{0:X4}", (int)c);
                    else
                        sb.Append(c);
                    break;
            }
        }

        return sb.ToString();
    }
    public static string ToXmlEscaped(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return input.Replace("&", "&amp;")
                    .Replace("\"", "&quot;")
                    .Replace("'",  "&apos;")
                    .Replace("<",  "&lt;")
                    .Replace(">",  "&gt;");
    }

    #region Private tools

    private static Random rng = new ();

    private static string BytesToHex(byte[] input)
    {
        return Convert.ToHexString(input).ToLower();
    }

    private static readonly Dictionary<char, string> MorseAlphabet = new()
                                                                     {
                                                                         {'A', ".-"}, {'B', "-..."}, {'C', "-.-."}, {'D', "-.."}, {'E', "."},
                                                                         {'F', "..-."}, {'G', "--."}, {'H', "...."}, {'I', ".."}, {'J', ".---"},
                                                                         {'K', "-.-"}, {'L', ".-.."}, {'M', "--"}, {'N', "-."}, {'O', "---"},
                                                                         {'P', ".--."}, {'Q', "--.-"}, {'R', ".-."}, {'S', "..."}, {'T', "-"},
                                                                         {'U', "..-"}, {'V', "...-"}, {'W', ".--"}, {'X', "-..-"}, {'Y', "-.--"},
                                                                         {'Z', "--.."}, {'1', ".----"}, {'2', "..---"}, {'3', "...--"}, {'4', "....-"},
                                                                         {'5', "....."}, {'6', "-...."}, {'7', "--..."}, {'8', "---.."}, {'9', "----."},
                                                                         {'0', "-----"}
                                                                     };

    private static readonly Dictionary<string, char> MorseDecode = MorseAlphabet.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    #endregion
}