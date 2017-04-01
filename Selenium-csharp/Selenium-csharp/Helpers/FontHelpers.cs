using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_csharp.Helpers
{
    /// <summary>
    /// This class helps using comparison operators on font sizes
    /// </summary>
    public class FontHelpers
    {
        /// <summary>
        /// Return font size value without "px"
        /// </summary>
        /// <param name="fontSize">Input font size in (value + px) format</param>
        public static double GetFontSizeValue(string fontSize)
        {
            string fontSizeValue = "";

            for (int i = 0; i < fontSize.Length; i++)
            {
                string ch = fontSize.Substring(i, 1);

                if (ch == ".")
                {
                    ch = ",";
                }

                if (ch == "p")
                {
                    break;
                }

                fontSizeValue += ch;
            }
                        
            return double.Parse(fontSizeValue);
        }
    }
}
