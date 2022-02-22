namespace Styleline.WinAnalyzer.AnalyzerLib
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class Utility
    {
        public static int BinToInt(string binaryNumber)
        {
            switch (binaryNumber)
            {
                case "0000":
                    return 0;

                case "0001":
                    return 1;

                case "0010":
                    return 2;

                case "0011":
                    return 3;

                case "0100":
                    return 4;

                case "0101":
                    return 5;

                case "0110":
                    return 6;

                case "0111":
                    return 7;

                case "1000":
                    return 8;

                case "1001":
                    return 9;
            }
            return -1;
        }

        public static void ParseNumber(string fullString, bool hertz, out decimal num)
        {
            num = 0M;
            decimal mult = 1M;
            StringBuilder sb = new StringBuilder();
            int[] nums = new int[4];
            string strFormat = "";
            string someString = fullString.Substring(14, 2);
            if (someString != null)
            {
                if (!(someString == "00"))
                {
                    if (someString == "10")
                    {
                        strFormat = "{0}{1}.{2}{3}";
                    }
                    else if (someString == "01")
                    {
                        strFormat = "{0}{1}{2}.{3}";
                    }
                    else if (someString == "11")
                    {
                        strFormat = "{0}.{1}{2}{3}";
                    }
                }
                else
                {
                    strFormat = "{0}{1}{2}{3}";
                }
            }
            if (hertz)
            {
                if (fullString.StartsWith("0"))
                {
                    mult = 1000M;
                }
                else
                {
                    mult = 1000000M;
                }
            }
            else if (fullString.StartsWith("0"))
            {
                sb.Append("-");
            }
            nums[0] = (fullString.Substring(1, 1) == "1") ? 1 : 0;
            nums[1] = BinToInt(fullString.Substring(2, 4));
            nums[2] = BinToInt(fullString.Substring(6, 4));
            nums[3] = BinToInt(fullString.Substring(10, 4));
            sb.AppendFormat(strFormat, new object[] { nums[0], nums[1], nums[2], nums[3] });
            decimal.TryParse(sb.ToString(), out num);
            num *= mult;
        }

        public static string Reverse(string s, int l)
        {
            if (l == 1)
            {
                return s;
            }
            return (Reverse(s.Substring(1, s.Length - 1), --l) + s[0]);
        }
    }
}

