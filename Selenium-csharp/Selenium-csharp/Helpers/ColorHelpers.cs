using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_csharp.Helpers
{
    // TODO:Rewrite using regular expression and Split, becouse not run in Firefox
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
            bool isCanalValue = false;
            string CanalValue = "";
            int countSpaces = 0;

            switch (canal)
            {
                case "r":
                case "R":
                    for (int i = 0; i < rgbColor.Length; i++)
                    {
                        string ch = rgbColor.Substring(i, 1);

                        if (ch == ",")
                        {
                            isCanalValue = false;
                        }

                        if (isCanalValue)
                        {
                            CanalValue += ch;
                        }

                        if (ch == "(")
                        {
                            isCanalValue = true;
                        }
                    }
                    break;

                case "g":
                case "G":
                    for (int i = 0; i < rgbColor.Length; i++)
                    {
                        string ch = rgbColor.Substring(i, 1);

                        if (ch == ",")
                        {
                            isCanalValue = false;
                        }

                        if (isCanalValue)
                        {
                            CanalValue += ch;
                        }

                        if (ch == " ")
                        {
                            countSpaces++;
                        }

                        if (ch == " " && countSpaces == 1)
                        {
                            isCanalValue = true;
                        }
                    }
                    break;

                case "b":
                case "B":
                    for (int i = 0; i < rgbColor.Length; i++)
                    {
                        string ch = rgbColor.Substring(i, 1);

                        if (ch == ",")
                        {
                            isCanalValue = false;
                        }

                        if (isCanalValue)
                        {
                            CanalValue += ch;
                        }

                        if (ch == " ")
                        {
                            countSpaces++;
                        }

                        if (ch == " " && countSpaces == 2)
                        {
                            isCanalValue = true;
                        }
                    }
                    break;

                default:
                    break;
            }
            return Int32.Parse(CanalValue);
        }
    }
}
