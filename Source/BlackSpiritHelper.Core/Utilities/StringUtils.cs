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
        /// <param name="dashes">Are dashes allowed?</param>
        /// <returns></returns>
        public static bool CheckAlphanumeric(this string input, bool underscores = false, bool spaces = false, bool dashes = false)
        {
            string regChars = "";

            if (underscores)
                regChars += "_";

            if (spaces)
                regChars += " ";

            if (dashes)
                regChars += "-";

            return Regex.IsMatch(input, @"^[a-zA-Z0-9" + regChars + @"]+$");
        }

        /// <summary>
        /// Check if the string has appropriate form as HEX color.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="hasHashmark"></param>
        /// <returns></returns>
        public static bool CheckColorHEX(this string str, bool hasHashmark = false)
        {
            if (hasHashmark)
                return Regex.IsMatch(str, @"^#[A-Fa-f0-9]{6}$");

            return Regex.IsMatch(str, @"^[A-Fa-f0-9]{6}$");
        }
    }
}
