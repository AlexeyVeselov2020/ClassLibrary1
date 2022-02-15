using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary
{
    internal static class Coding
    {
        public static string EncodeRequest(string request)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(request);
            StringBuilder result = new StringBuilder();
            int i= 0;
            while (i < bytes.Length)
            {
                string str = Convert.ToString(bytes[i], 2);
                for(int j=str.Length;j<8;j++)
                    result.Append("0");
                result.Append(str);
                i++;
            }
            return result.ToString();
        }

        public static string AddingServiceFields(string data, int correction_level, out int version_number)
        {
            string length = Convert.ToString(data.Length/8, 2);
            version_number = default;
            for (int i = 0; i < 40; i++)
            {
                if (Tables.MaximumAmountofInformation[correction_level, i] > data.Length)
                {
                    version_number = i;
                    break;
                }
            }
            if (version_number < 9)
            {
                if (Tables.MaximumAmountofInformation[correction_level, version_number] < data.Length + 12)
                    version_number++;
                while (length.Length < 8)
                    length = "0" + length;
            }
            else
            {
                if (Tables.MaximumAmountofInformation[correction_level, version_number] < data.Length + 20)
                    version_number++;
                while (length.Length < 16)
                    length = "0" + length;
            }
            bool typeofaddingbyte = true;
            StringBuilder result = new StringBuilder("0100" + length + data + "0000");
            while (result.Length < Tables.MaximumAmountofInformation[correction_level, version_number])
            { 
                if (typeofaddingbyte)
                {
                    result.Append("11101100");
                    typeofaddingbyte = false;
                }
                else
                {
                    result.Append("00010001");
                    typeofaddingbyte = true;
                }
            }
            return result.ToString();
        }

        public static string[] SplittingintoBlocks(string data, int correction_level, int version_number)
        {
            int numberofblocks = Tables.NumberofInformationBlocks[correction_level, version_number];
            int amountofbytesinblock = (Tables.MaximumAmountofInformation[correction_level, version_number] / 8) / numberofblocks;
            int numberofcrowdedblocks = (Tables.MaximumAmountofInformation[correction_level, version_number] / 8) % numberofblocks;
            var result = new string[numberofblocks];
            int i = 0,j = 0;
            while(numberofblocks>0)
            {
                if (numberofblocks <= numberofcrowdedblocks)
                {
                    result[i] = data.Substring(j, (amountofbytesinblock+1)*8);
                    j += (amountofbytesinblock + 1)*8;
                }
                else
                {
                    result[i] = data.Substring(j, amountofbytesinblock*8);
                    j += amountofbytesinblock*8;
                }
                i++;
                numberofblocks--;
            }
            return result;
        }

        public static string CreatingCorrectionBytes(string[] blocks, int correction_level, int version_number)
        {
            int amount_of_correction_bytes = Tables.NumberofCorrectionBytesperBlock[correction_level, version_number];
            int[] generating_polynomial = Tables.GeneratingPolynomials[amount_of_correction_bytes];
            var galois_field = GetGaloisField();
            var reverse_galois_field = GetReverseGaloisField(galois_field);
            var correction_blocks = new List<List<int>>();
            var data = new StringBuilder();
            for (int i = 0; i < blocks.Length; i++)
            {
                string corrent_block = blocks[i];
                int block_length = corrent_block.Length / 8;
                var correction_bytes = new int[Math.Max(amount_of_correction_bytes, block_length)];

                for (int j = 0; j < correction_bytes.Length; j++)
                {
                    if (j >= block_length)
                        correction_bytes[j] = 0;
                    else
                        correction_bytes[j] = (byte)BinaryToDecimal(corrent_block.Substring(j * 8, 8));
                }

                for (int j = 0; j < block_length; j++)
                {
                    int corrent_byte = correction_bytes[0];
                    for (int k = 1; k < correction_bytes.Length; k++)
                        correction_bytes[k - 1] = correction_bytes[k];
                    correction_bytes[correction_bytes.Length - 1] = 0;
                    if (corrent_byte != 0)
                    {
                        corrent_byte = reverse_galois_field[corrent_byte];
                        for (int k = 0; k < amount_of_correction_bytes; k++)
                        {
                            int a = (corrent_byte + generating_polynomial[k]) % 255;
                            correction_bytes[k] = (byte)(correction_bytes[k] ^ galois_field[a]);
                        }
                    }
                }
                var l = new List<int>();
                for (int j = 0; j < amount_of_correction_bytes; j++)
                    l.Add(correction_bytes[j]);
                correction_blocks.Add(l);
            }

            for (int i = 0; i < blocks[0].Length / 8 + 1; i++)
            {
                for (int j = 0; j < blocks.Length; j++)
                    if (i * 8 < blocks[j].Length)
                        data.Append(blocks[j].Substring(i * 8, 8));
            }

            for (int i = 0; i < correction_blocks[0].Count ; i++)
            {
                for (int j = 0; j < correction_blocks.Count; j++)
                {
                    string str = Convert.ToString(correction_blocks[j][i], 2);
                    for (int k = str.Length; k < 8; k++)
                        data.Append("0");
                    data.Append(str);
                }
            }
            return data.ToString();
        }

        public static int[] GetGaloisField()
        {
            int[] field = new int[256];
            field[255] = 0;
            int el = 1;
            for(int i=0;i<255;i++)
            {
                field[i] = el;
                el *= 2;
                if (el > 255)
                    el = el ^ 285;
            }
            return field;
        }
        static uint BinaryToDecimal(string binaryNumber)
        {
            var exponent = 0;
            var result = 0u;
            for (var i = binaryNumber.Length - 1; i >= 0; i--)
            {
                if (binaryNumber[i] == '1')
                {
                    result += Convert.ToUInt32(Math.Pow(2, exponent));
                }
                exponent++;
            }
            return result;
        }

        public static int[] GetReverseGaloisField(int[] field)
        {
            int[] corrent_field=new int[256];
            Array.Copy(field, corrent_field,256);
            var dic = new Dictionary<int, int>();
            for (int i = 0; i < 256; i++)
            {
                dic.Add(i, corrent_field[i]);
            }
            Array.Clear(corrent_field, 0,256);
            foreach (var el in dic)
                corrent_field[el.Value] = el.Key;
            return corrent_field;
        }
    }
}
