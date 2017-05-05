using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace FashionStore.Code
{
	public static class Extensions
	{
		public static string GetDescription(this Enum en)
		{
			Type type = en.GetType();
			MemberInfo[] memInfo = type.GetMember(en.ToString());

			if (memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (attrs.Length > 0)
					return ((DescriptionAttribute)attrs[0]).Description;
			}
			return en.ToString();
		}

        public static string GetDisplayName(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DisplayNameAttribute), false);
                if (attrs.Length > 0)
                    return ((DisplayNameAttribute)attrs[0]).DisplayName;
            }
            return en.ToString();
        }

		public static string ToUpperFirst(this string value)
		{
			if (String.IsNullOrWhiteSpace(value))
				return String.Empty;
			if(value.ToLower() == "null")
				return "N.A.";

			// selects extra spaces between words
			const string whitespace = @"\s{2,}";
			// selects 1 character after space, dash & underscore to capitalize
			const string startOfWord = @"^\w{1}|[\s_-]{1}\w{1}";
			// selects words containing alphanumeric with or without dash to capitalize. This is done for model and IMEI numbers.
			const string wordWithNumber = @"(?<=^|\s|\.)[\w-]*\d+[\w-]*(?=$|\s|\.)";
			// selects only alpha words contain 
			//const string minLengthWord = @"(?<=^|\s|\.)\w{3}(?=$|\s|\.)";

			value = value.Trim().ToLower();
			value = Regex.Replace(value, whitespace, " ");
			value = Regex.Replace(value, startOfWord, MatchEvaluator);
			value = Regex.Replace(value, wordWithNumber, MatchEvaluator);

			return value;
		}

		private static string MatchEvaluator(Match match)
		{
			return match.Value.ToUpper();
		}

		public static bool IsInt32(this string value)
		{
			int i;
			return Int32.TryParse(value, out i);
		}

		public static bool IsNumber(this string value)
		{
			const string pattern = @"\d+";
			return Regex.IsMatch(value, pattern, RegexOptions.Singleline);
		}

		public static bool IsNumber(this string value, int length)
		{
			string pattern = String.Format(@"\d{{{0}}}", length);
			return Regex.IsMatch(value, pattern, RegexOptions.Singleline);
		}
	}
}
