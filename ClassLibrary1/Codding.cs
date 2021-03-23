using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary
{
    public static class Codding
    {
        public static string EncodeRequest(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            StringBuilder result = new StringBuilder();
            int i= 0;
            while (i < bytes.Length)
            {
                result.Append(Convert.ToString(bytes[i],2));
                i++;
            }
            return result.ToString();
        }
        
        public static string AddingServiceFields(string str, int correction_level)
        {
            string result = Convert.ToString(str.Length, 2);
            int version_number=default;
            for(int i=0;i<40;i++)
            {
                if (Tables.MaximumAmountofInformation1[correction_level, i] > str.Length)
                {
                    version_number = i;
                    break;
                }
            }
            if(version_number<10)
            {
                if (Tables.MaximumAmountofInformation1[correction_level, version_number] < str.Length + 12)
                    version_number++;
                while (result.Length < 8)
                    result = "0" + result;
            }
            else
            {
                if (Tables.MaximumAmountofInformation1[correction_level, version_number] < str.Length + 20)
                    version_number++;
                while (result.Length < 16)
                    result = "0" + result;
            }
            result = "0100" + result + str + "0000";
            return result;
        }
    }
    
    //public interface IDictionary
    //{
    //    Dictionary<char,int> Table { get; set; }

    //    IDictionary EncodeIt();
    //}

    //public class NumericEncodding : IDictionary
    //{
    //    public Dictionary<char, int> Table { get; set; }

    //    public IDictionary EncodeIt()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class AlphanumericEncoding : IDictionary
    //{
    //    public Dictionary<char, int> Table { get; set; }

    //    public IDictionary EncodeIt()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
