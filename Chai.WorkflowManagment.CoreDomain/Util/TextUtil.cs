using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Util
{
    public class TextUtil
    {
        private TextUtil()
        {
        }


        public static string TruncateText(string fullText, int numberOfCharacters)
        {
            string text;
            if (fullText.Length > numberOfCharacters)
            {
                int spacePos = fullText.IndexOf(" ", numberOfCharacters);
                if (spacePos > -1)
                {
                    text = fullText.Substring(0, spacePos) + "...";
                }
                else
                {
                    text = fullText;
                }
            }
            else
            {
                text = fullText;
            }
            //Regex regexStripHTML = new Regex("<[^>]+>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //text = regexStripHTML.Replace(text, " ");
            return text;
        }


        public static string EnsureTrailingSlash(string stringThatNeedsTrailingSlash)
        {
            if (!stringThatNeedsTrailingSlash.EndsWith("/"))
            {
                return stringThatNeedsTrailingSlash + "/";
            }
            else
            {
                return stringThatNeedsTrailingSlash;
            }
        }

        public static bool IsDigit(string strtocheck, Type type)
        {
            if (string.IsNullOrEmpty(strtocheck))
                return false;

            if (type.Name == "Int32" || type.Name == "int")
            {
                try
                {
                    int.Parse(strtocheck);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            if (type.Name == "Decimal")
            {
                try
                {
                    decimal.Parse(strtocheck);
                    return true;
                }
                catch { return false; }
            }
            if (type.Name == "Double")
            {
                try
                {
                    double.Parse(strtocheck);
                    return true;
                }
                catch { return false; }
            }

            if (type.Name == "Float")
            {
                try
                {
                    float.Parse(strtocheck);
                    return true;
                }
                catch { return false; }

            }
            return false;
        }


    }
}
