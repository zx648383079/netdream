using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NetDream.Shared.Helpers
{
    public static partial class Validator
    {

        public static bool IsNotEmpty(string? value) 
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsLowercase(string input) =>
            input == input.ToLower();
        public static bool IsUppercase(string input) =>
           input == input.ToUpper();

        public static bool IsInt(string input) => IsNumeric(input);

        public static bool IsFloat(string input)
        {
            return float.TryParse(input, out var _);
        }

        public static bool IsLength(string input, int min, int max) =>
            input.Length > min && input.Length < max;

        public static bool IsAscii(string input) =>
            input.Select(c => (int)c).All(c => c <= 127);

        public static bool IsIn(string input, string[] values) =>
            values.Any(value => value == input);

        public static bool IsMobile(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            return MobileRegex().IsMatch(value);
        }

        public static bool IsEmail(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            try
            {
                return new MailAddress(value).Address == value;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsIp(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            if (input.Contains(':'))
            {
                return IPv6Regex().IsMatch(input);
            }
            if (!IPv4Regex().IsMatch(input))
            {
                return false;
            }
            var parts = input.Split('.').Select(p => Convert.ToInt32(p));
            return parts.Max() <= 255;
        }


        public static bool IsNumeric(string? value)
        {
            if (value == null)
            {
                return false;
            }

            var length = value.Length;
            if (length == 0)
            {
                return false;
            }

            var i = 0;
            if (value[0] == '-')
            {
                if (length == 1)
                {
                    return false;
                }

                i = 1;
            }

            for (; i < length; i++)
            {
                char c = value[i];
                if (c <= '/' || c >= ':')
                {
                    return false;
                }
            }

            return true;
        }

        [GeneratedRegex(@"^(0|86|17951)?(13[0-9]|15[012356789]|17[013678]|18[0-9]|19[89]|14[57])[0-9]{8}$")]
        private static partial Regex MobileRegex();
        [GeneratedRegex(@"^(\d?\d?\d)\.(\d?\d?\d)\.(\d?\d?\d)\.(\d?\d?\d)$")]
        private static partial Regex IPv4Regex();
        [GeneratedRegex(@"^::|^::1|^([a-fA-F0-9]{1,4}::?){1,7}([a-fA-F0-9]{1,4})$")]
        private static partial Regex IPv6Regex();

        public static bool IsBoolean(string content)
        {
            return content.ToLower() is "true" or "false";
        }

        public static bool IsDouble(string content)
        {
            return double.TryParse(content, out var _);
        }
    }
}
