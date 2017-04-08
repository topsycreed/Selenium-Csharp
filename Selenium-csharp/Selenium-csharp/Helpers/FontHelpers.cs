using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static int GetFontSizeValue(string fontSize)
        {
            //Get one or more digital numbers
            var regex = new Regex(@"^\d+");
            string result = regex.Match(fontSize).ToString();
                        
            return int.Parse(result);
        }
    }
}
