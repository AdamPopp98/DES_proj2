using System;


namespace DesProject
{
    public static class s_box
    {

        public static bool[] compression(ByteContainer input_ByteContainer)
        {
			int[,] box1 = new int[,] {{1, 0, 3, 2}, {3, 2, 1, 0}, {0, 2, 1, 3}, {3, 1, 3, 2}};
			int[,] box2 = new int[,] {{0, 1, 2, 3}, {2, 0, 1, 3}, {3, 0, 1, 0}, {2, 1, 0, 3}};
            //Splits the input byte into its left and right halves.
            HalfByteContainer left = input_ByteContainer.split_ByteContainer().Item1;
            HalfByteContainer right = input_ByteContainer.split_ByteContainer().Item2;
            //the coordnates to be used as input for the 2 sboxes are obtained
            Tuple<int, int> box1_input = get_matrix_coords(left);
            Tuple<int, int> box2_input = get_matrix_coords(right);
            //the specified coordinates for the sboxes are input and the resulting integer values are stored.
            int box1_result = get_matrix_val(box1_input, box1);
            int box2_result = get_matrix_val(box2_input, box2);
            //the integer values that were returned by the sboxes are converted to bit pairs.
            Tuple<bool, bool> left_bits = get_bit_vals(box1_result);
            Tuple<bool, bool> right_bits = get_bit_vals(box2_result);
            //Finally the bit pairs are combined into a 4 bit array and returned.
            return combine_bit_pairs(left_bits, right_bits);        
        }

        //Used to convert a 4 bit input into a tuple containing 2 integers which represent the row and column within an sbox;
        public static Tuple<int, int> get_matrix_coords(HalfByteContainer read_from)
        {
            bool[] bits = read_from.get_bits();
            //the first and last bit become a bit pair representing the sbox row number in binary
            Tuple<bool, bool> row = Tuple.Create(bits[0], bits[3]);
            //the middle 2 bits become a bit pair representing the sbox column number in binary
            Tuple<bool, bool> col = Tuple.Create(bits[1], bits[2]);
            //the row and column are converted to integer representations which are returned as a tuple.
            return Tuple.Create(convert_to_int(row), convert_to_int(col));
        }

        //Used to convert 2 bits of binary into a decimal value to represent the row or column within an sbox
        private static int convert_to_int(Tuple<bool, bool> bit_pair)
        {
			int sum = 0;
			if (bit_pair.Item1)
			{
				sum += 2;
			}
			if (bit_pair.Item2)
			{
				sum++;
			}
			return sum;
        }

        //returns the value located in an sbox at a given row and column.
        public static int get_matrix_val(Tuple<int, int> coords, int[,] s_box_matrix)
        {
            return s_box_matrix[ coords.Item1, coords.Item2 ];
        }

        //Converts an integer value obtained from an sbox into a bit pair representation.
        public static Tuple<bool, bool> get_bit_vals(int integer_val)
        {
            switch (integer_val)
            {
                case 0:
                    return Tuple.Create(false, false);
                case 1:
                    return Tuple.Create(false, true);
                case 2:
                    return Tuple.Create(true, false);
                default:
                    return Tuple.Create(true, true);
            }
        }

        //Combines the two bit pairs obtained from the sboxes into a single 4 bit array.
        public static bool[] combine_bit_pairs(Tuple<bool, bool> left, Tuple<bool, bool> right)
        {
            bool[] bits = { left.Item1, left.Item2, right.Item1, right.Item2 };
            return bits;
        }
    }
}