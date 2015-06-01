using System;
using System.Text;

namespace WIT.Common.Randomizer
{
    /// <summary>
    /// Helper class used for random numbers generation.
    /// </summary>
    public class Randomizer
    {
        /// <summary>
        /// Random index generator.
        /// </summary>
        public static Random Random;

        /// <summary>
        /// Default alphabet.
        /// </summary>
        private static char[] DEFAULT_ALPHABET = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Generate alphanumeric code.
        /// </summary>
        /// <param name="length">Length of the desired code.</param>
        /// <param name="alphabet">Alphabet to be used for the code generation.</param>
        /// <returns>Random alphanumeric code.</returns>
        public static string GetRandomAlphanumericCode(int length)
        {
            return GetRandomAlphanumericCode(length, DEFAULT_ALPHABET);
        }

        /// <summary>
        /// Generate alphanumeric code.
        /// </summary>
        /// <param name="length">Length of the desired code.</param>
        /// <returns>Random alphanumeric code.</returns>
        public static string GetRandomAlphanumericCode(int length, char[] alphabet)
        {
            StringBuilder code;

            code = new StringBuilder();

            for (int j = 0; j < length; j++)
            {
                code.Append(alphabet[Random.Next(alphabet.Length)]);
            }

            return code.ToString();
        }
    }
}
