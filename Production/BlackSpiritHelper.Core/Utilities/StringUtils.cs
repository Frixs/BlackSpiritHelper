using System.Text.RegularExpressions;

namespace BlackSpiritHelper.Core
{
    public static class StringUtils
    {
        /// <summary>
        /// Check if the string contains only letters and numbers.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <param name="underscores">Are underscores allowed?</param>
        /// <param name="spaces">Are spaces allowed?</param>
        /// <returns></returns>
        public static bool CheckAlphanumericString(string input, bool underscores = false, bool spaces = false)
        {
            if (underscores && spaces)
                return Regex.IsMatch(input, @"^[a-zA-Z0-9_ ]+$");

            if (underscores)
                return Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$");

            if (spaces)
                return Regex.IsMatch(input, @"^[a-zA-Z0-9 ]+$");

            return Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$");
        }

    }
}
