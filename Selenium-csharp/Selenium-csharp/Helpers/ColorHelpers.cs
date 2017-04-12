using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Selenium_csharp.Helpers
{
    /// <summary>  
    /// This class help convert RGBa color into separate canal value and check what it color is.  
    /// </summary> 
    public static class ColorHelpers
    {
        /// <summary>  
        /// Return true, if color is red (canal R > 0, others canals equals 0).
        /// </summary> 
        /// <param name="rgbColor">Input color in RGBa format</param>
        public static bool isRed(string rgbColor)
        {
            int R = GetCanalValue(rgbColor, "R");
            int G = GetCanalValue(rgbColor, "G");
            int B = GetCanalValue(rgbColor, "B");

            if (((G & B) == 0) & (R > 0))
            {
                return true;
            }
            return false;
        }

        /// <summary>  
        /// Return true, if color is gray (all canals are equal and > 0).
        /// </summary> 
        /// <param name="rgbColor">Input color in RGBa format</param>
        public static bool isGray(string rgbColor)
        {
            int R = GetCanalValue(rgbColor, "R");
            int G = GetCanalValue(rgbColor, "G");
            int B = GetCanalValue(rgbColor, "B");

            if ((R == G) & (G == B))
            {
                return true;
            }

            return false;
        }

        /// <summary>  
        /// Return value of canal
        /// </summary> 
        /// <param name="rgbColor">Input color in RGBa format</param>
        /// <param name="rgbColor">Input canal</param>
        public static int GetCanalValue(string rgbColor, string canal)
        {
            //Match only digital numbers
            MatchCollection matchList = Regex.Matches(rgbColor, @"\d+");
            var list = matchList.Cast<Match>().Select(match => match.Value).ToList();

            string CanalValue = "";

            switch (canal)
            {
                case "r":
                case "R":
                    CanalValue = list[0].ToString();
                    break;

                case "g":
                case "G":
                    CanalValue = list[1].ToString();
                    break;

                case "b":
                case "B":
                    CanalValue = list[2].ToString();
                    break;

                default:
                    throw new System.ArgumentException("Parameter canal cannot be null or not RGB", "canal");
            }
            return Int32.Parse(CanalValue);
        }
    }
}
