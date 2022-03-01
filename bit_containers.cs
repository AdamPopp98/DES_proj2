using System;


namespace DesProject {

	public class BitContainer
	{
		protected bool[] bits;

		public bool[] get_bits()
        {
            return bits;
        }

        //Used to change the values stored in the bits member variable of a BitContainer.
		public void set_bits(bool[] new_bits)
        {
            if (bits.Length != new_bits.Length)
            {
                Console.WriteLine("Cannot set bits using a bit array of a different length");
                return;
            }
            bits = new_bits;
        }

        //Used to change the values stored in the bits member variable of a BitContainer.
        public void set_bits(string binary_string)
        {
            if (bits.Length != binary_string.Length)
            {
                Console.WriteLine("Cannot set bits using a binary string of a different length");
                return;
            }
            for (int i = 0; i < bits.Length; i ++)
            {
                if (binary_string[i] == '1')
                {
                    bits[i] = true;
                }
                else
                {
                    bits[i] = false;
                }
            }
        }

        //Used to return the binary string representation of a BitContainers bits member variable
		public string as_binary_string()
        {
            string binary_string = "";
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == true)
                {
                    binary_string += '1';
                }
                else
                {
                    binary_string += '0';
                }
            }
            return binary_string;
        }

        //Used to xor bits with another bit array of the same size.
		public void xor_bits(bool[] xor_with)
        {
			if (xor_with.Length != bits.Length)
			{
				Console.WriteLine("Error: Cannot xor bit arrays of different lengths.\nAttempted to xor bit array of length: " + bits.Length + ", with bit array of length: " + xor_with.Length + ".");
				return;
			}
            for (int i = 0; i < 4; i++)
            {
                if (bits[i] == xor_with[i])
                {
                    bits[i] = false;
                }
                else
                {
                    bits[i] = true;
                }
            }
        }

		public void increment_bits()
        {
            bool carry_bit = true;
            int bit_num = bits.Length - 1;
            while (carry_bit == true)
            {
                bits[bit_num] = !bits[bit_num];
                if (bit_num == 0 || bits[bit_num] == true)
                {
                    return;
                }
				bit_num--;
            }
        }       

        public void right_shift_bits()
        {
            bool temp = bits[0];
            bool temp2 = bits[0];
            bits[0] = false;
            for (int i = 0; i < bits.Length - 1; i++)
            {
                temp2 = bits[i + 1];
                bits[i + 1] = temp;
                temp = temp2;
            }
        }

        public void left_shift_bits()
        {
            bool temp = bits[(bits.Length - 1)];
            bool temp2 = bits[(bits.Length - 1)];
            bits[bits.Length - 1] = false;
            for (int i = bits.Length - 1; i > 0; i--)
            {
                temp2 = bits[i - 1];
                bits[i - 1] = temp;
                temp = temp2;
            }
        }

        public void rotate_bits(int num_rotations = 1)
        {
            while (num_rotations > 0)
            {
                bool temp = bits[bits.Length - 1];
                bool temp2 = bits[bits.Length - 1];
                bits[bits.Length - 1] = bits[0];
                for (int i = bits.Length - 1; i > 0; i--)
                {
                    temp2 = bits[i - 1];
                    bits[i - 1] = temp;
                    temp = temp2;
                }
                num_rotations--;
            }
        }
	}
    public class TenBitKey : BitContainer
    {
		public TenBitKey()
		{
			bits = new bool[] { false, false, false, false, false, false, false, false, false, false };
		}

		public TenBitKey(bool[] bit_arr)
        {
            bits = bit_arr;
        }

        public TenBitKey(string binary_string)
        {
            bits = new bool[10];
            for (int i = 0; i < bits.Length; i++)
            {
                if (binary_string[i] == '1')
                {
                    bits[i] = true;
                }
                else
                {
                    bits[i] = false;
                }
            }
        }

        //Splits the TenBitKey into two HalfKeys and permutes them using the PC1 table
        public Tuple<HalfKey, HalfKey> split_key()
        {
            HalfKey c = new HalfKey(new bool[] { bits[2], bits[4], bits[1], bits[6], bits[3] });
            HalfKey d = new HalfKey(new bool[] { bits[9], bits[0], bits[8], bits[7], bits[5] });
            Console.WriteLine("Key: " + as_binary_string() + " C: " + c.as_binary_string() + " D: " + d.as_binary_string());
            return Tuple.Create(c, d);
        }
    }

    public class HalfKey : BitContainer
    {
		public HalfKey()
		{
			bits = new bool[] { false, false, false, false, false };			
		}

		public HalfKey(bool[] bit_arr)
        {
            bits = bit_arr;
        }
    }

    public class ByteContainer : BitContainer
    {
        public ByteContainer()
        {
            bits = new bool[] {false, false, false, false, false, false, false, false };
        }

		public ByteContainer(bool[] bit_arr)
        {
            bits = bit_arr;
        }

        //Converts a pair of Hexadecimal characters into an 8 bit array.
        public ByteContainer(string hex)
        {
            bool[] first_half;
            bool[] second_half;
            switch (hex[0])
            {
                case '0':
                    first_half = new bool[] {false, false, false, false};
                    break;
                case '1':
                    first_half = new bool[] {false, false, false, true};
                    break;
                case '2':
                    first_half = new bool[] {false, false, true, false};
                    break;
                case '3':
                    first_half = new bool[] {false, false, true, true};
                    break;
                case '4':
                    first_half = new bool[] {false, true, false, false};
                    break;
                case '5':
                    first_half = new bool[] {false, true, false, true};
                    break;
                case '6':
                    first_half = new bool[] {false, true, true, false};
                    break;
                case '7':
                    first_half = new bool[] {false, true, true, true};
                    break;
                case '8':
                    first_half = new bool[] {true, false, false, false};
                    break;
                case '9':
                    first_half = new bool[] {true, false, false, true};
                    break;
                case 'a':
                    first_half = new bool[] {true, false, true, false};
                    break;
                case 'b':
                    first_half = new bool[] {true, false, true, true};
                    break;
                case 'c':
                    first_half = new bool[] {true, true, false, false};
                    break;
                case 'd':
                    first_half = new bool[] {true, true, false, true};
                    break;
                case 'e':
                    first_half = new bool[] {true, true, true, false};
                    break;
                default:
                    first_half = new bool[] {true, true, true, true};
                    break;
            }
            switch (hex[1])
            {
                case '0':
                    second_half = new bool[] {false, false, false, false};
                    break;
                case '1':
                    second_half = new bool[] {false, false, false, true};
                    break;
                case '2':
                    second_half = new bool[] {false, false, true, false};
                    break;
                case '3':
                    second_half = new bool[] {false, false, true, true};
                    break;
                case '4':
                    second_half = new bool[] {false, true, false, false};
                    break;
                case '5':
                    second_half = new bool[] {false, true, false, true};
                    break;
                case '6':
                    second_half = new bool[] {false, true, true, false};
                    break;
                case '7':
                    second_half = new bool[] {false, true, true, true};
                    break;
                case '8':
                    second_half = new bool[] {true, false, false, false};
                    break;
                case '9':
                    second_half = new bool[] {true, false, false, true};
                    break;
                case 'a':
                    second_half = new bool[] {true, false, true, false};
                    break;
                case 'b':
                    second_half = new bool[] {true, false, true, true};
                    break;
                case 'c':
                    second_half = new bool[] {true, true, false, false};
                    break;
                case 'd':
                    second_half = new bool[] {true, true, false, true};
                    break;
                case 'e':
                    second_half = new bool[] {true, true, true, false};
                    break;
                default:
                    second_half = new bool[] {true, true, true, true};
                    break;
            }
            bits = new bool[] { first_half[0], first_half[1], first_half[2], first_half[3], second_half[0], second_half[1], second_half[2], second_half[3] };
        }

        public static bool operator== (ByteContainer lhs, ByteContainer rhs)
        {
            bool[] left_bits = lhs.get_bits();
            bool[] right_bits = rhs.get_bits();
            if (left_bits.Length != right_bits.Length)
            {
                return false;
            }
            for (int i = 0; i < left_bits.Length; i++)
            {
                if (left_bits[i] != right_bits[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator!= (ByteContainer lhs, ByteContainer rhs)
        {
            bool[] left_bits = lhs.get_bits();
            bool[] right_bits = rhs.get_bits();
            if (left_bits.Length != right_bits.Length)
            {
                return true;
            }
            for (int i = 0; i < left_bits.Length; i++)
            {
                if (left_bits[i] != right_bits[i])
                {
                    return true;
                }
            }
            return false;
        }

        public void initial_permutaion()
        {
            bool[] new_bits = { bits[1], bits[5], bits[2], bits[0], bits[3], bits[7], bits[4], bits[6] };
            bits = new_bits;
        }

        public void final_permutaion(HalfByteContainer left, HalfByteContainer right)
        {
            bool[] right_bits = left.get_bits();
            bool[] left_bits = right.get_bits();
            bool[] new_bits = { left_bits[3], left_bits[0], left_bits[2], right_bits[0], right_bits[2], left_bits[1], right_bits[3], right_bits[1] };
            bits = new_bits;
        }

        //Used to split a byte into a pair of half bytes which are used as inputs for the encryption algorithm
        public Tuple<HalfByteContainer, HalfByteContainer> split_ByteContainer()
        {
            bool[] left = { bits[0], bits[1], bits[2], bits[3] };
            bool[] right = { bits[4], bits[5], bits[6], bits[7] };
            return Tuple.Create(new HalfByteContainer(left), new HalfByteContainer(right));
        }
    }

    public class HalfByteContainer : BitContainer
    {
        public HalfByteContainer()
        {
            bits = new bool[] { false, false, false, false };
        }

		public HalfByteContainer(bool[] bit_arr)
        {
			bits = bit_arr;
        }

        //Used to expand a 4 bits into 8 so they can be xored with the round key
        public ByteContainer expand_bits()
        {
            bool[] expanded_bits = { bits[3], bits[0], bits[1], bits[2], bits[1], bits[2], bits[3], bits[0] };
            return new ByteContainer(expanded_bits);
        }

        //Used to permute the 4 bits that are output by the S boxes
        public void permute_bits()
        {
            bool[] new_bits = { bits[1], bits[3], bits[2], bits[0] };
            bits = new_bits;
        }
    }
}