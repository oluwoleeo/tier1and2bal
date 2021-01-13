using System;
using System.Text.RegularExpressions;

namespace Tier1And2BalanceEnforcement
{
    public class Utility
    {
        public static string RedactString(string accountToRedact)
        {
            string finalStr = "";

            if (!string.IsNullOrWhiteSpace(accountToRedact))
            {
                char[] vals = accountToRedact.ToCharArray();
                char[] vals1 = { vals[0], vals[1], vals[2] };
                char[] vals2 = { vals[7], vals[8], vals[9] };
                
                finalStr += new String(vals1);
                finalStr += "XXXX";
                finalStr += new String(vals2);
            }

            return finalStr;
        }

        public static string ProcessPhone(string phoneNumber)
        {
            string finalStr = "";

            if (phoneNumber.StartsWith("0"))
            {
                finalStr = phoneNumber.Remove(0, 1);
                finalStr = finalStr.Insert(0, "234");
                finalStr = Regex.Replace(finalStr, "[()+]", "");
            }
            else
            {
                finalStr = Regex.Replace(phoneNumber, "[()+]", "");
            }

            return finalStr;
        }
    }
}
