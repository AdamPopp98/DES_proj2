using System;


namespace DesProject
{
    public static class FormatConverter
    {
        public static ByteContainer[] hex_to_ByteContainer_array(string input)
        {
            ByteContainer[] formatted_output = new ByteContainer[1];
            Array.Resize(ref formatted_output, (input.Length / 2));
            for (int i = 0, j = 0; i < input.Length; i += 2, j++)
            {
                string hex_pair = input.Substring(i, 2);
                Console.WriteLine(hex_pair);
                formatted_output[j] = new ByteContainer(hex_pair);
            }
            return formatted_output;
        }

        public static string ByteContainer_array_to_hex(ByteContainer[] input)
        {
            string formatted_output = "";
            for (int i = 0; i < input.Length; i++)
            {
                bool[] bits = input[i].get_bits();
                bool[] first_half = {bits[0], bits[1], bits[2], bits[3]};
                bool[] second_half = {bits[4], bits[5], bits[6], bits[7]};
                formatted_output += bits_to_hex(first_half);
                formatted_output += bits_to_hex(second_half);
            }
            return formatted_output;
        }

        //public string ascii_to_ByteContainer_array()

        public static string bits_to_hex(bool[] bits)
        {
            int sum = 0;
            if (bits[0])
            {
                sum += 8;
            }
            if (bits[1])
            {
                sum += 4;
            }
            if (bits[2])
            {
                sum += 2;
            }
            if (bits[3])
            {
                sum++;
            }
            return Convert.ToString(sum, 16);
        }
    }    
}