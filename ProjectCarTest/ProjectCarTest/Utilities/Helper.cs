using System.Text.RegularExpressions;

namespace ProjectCarTest.Utilities
{
    /// <summary>
    /// Helper class to have global access to any commonly used methods across the project.
    /// </summary>
    public class Helper
    {
        public static class StringValidator
        {
            // Valid input text should allow: A-Z, a-z, 0-9, @ & $ ! # ?
            private static readonly Regex legalCharsRegex = new Regex(@"^[A-Za-z0-9@&$!#?]+$");

            public static bool ContainsOnlyLegalCharacters(string input)
            {
                if (string.IsNullOrEmpty(input))
                    return false;

                return legalCharsRegex.IsMatch(input);
            }
        }

    }
}
