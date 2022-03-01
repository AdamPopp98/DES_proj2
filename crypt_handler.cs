using System;

namespace DesProject
{
    public static class CryptHandler
    {
		//Used to encrypt or decrypt a series of 8 bit blocks using a fiestal cipher.
        public static ByteContainer[] get_output_ecb(ByteContainer[] input_ByteContainers, TenBitKey crypt_key, bool is_plaintext = true)
        {
            ByteContainer[] output_ByteContainers = new ByteContainer[input_ByteContainers.Length];
			//Generates the round keys using the ten bit cryptographic key.
            ByteContainer[] round_keys = RoundKeyHandler.generate_round_keys(crypt_key, is_plaintext);
			//performs the fiestal cipher on each byte of the input.
            for (int i = 0; i < output_ByteContainers.Length; i++)
            {
                output_ByteContainers[i] = perform_algorithm(input_ByteContainers[i], round_keys);
            }
            return output_ByteContainers;
        }

		public static ByteContainer[] get_output_cbc(ByteContainer[] input_ByteContainers, TenBitKey crypt_key, ByteContainer IV, bool is_plaintext = true)
		{
			//the first next_iv value is the IV passed in as an argument
			bool[] next_iv = IV.get_bits();
			ByteContainer[] output_ByteContainers = new ByteContainer[input_ByteContainers.Length];
			//Generates the round keys using the ten bit cryptographic key.
		    ByteContainer[] round_keys = RoundKeyHandler.generate_round_keys(crypt_key, is_plaintext);
			for (int i = 0; i < output_ByteContainers.Length; i++)
			{
				output_ByteContainers[i] = new ByteContainer(input_ByteContainers[i].get_bits());
				//runs when encrypting
				if (is_plaintext == true)
				{
					//the plaintext is xored with next_iv
					output_ByteContainers[i].xor_bits(next_iv);
					//the algorithm is run on the xored block
					output_ByteContainers[i] = perform_algorithm(output_ByteContainers[i], round_keys);
					//the next_iv is set to the bit array of the resulting ciphertext
					next_iv = output_ByteContainers[i].get_bits();
				}
				//runs when decrypting
				else
				{
					//the algorithm is run on the ciphertext
					output_ByteContainers[i] = perform_algorithm(output_ByteContainers[i], round_keys);
					//the resulting plaintext is xored with next_iv
					output_ByteContainers[i].xor_bits(next_iv);
					//the next_iv is set to the current blocks ciphertext bit array.
					next_iv = input_ByteContainers[i].get_bits();
				}
			}
			return output_ByteContainers;
		}

        private static ByteContainer perform_algorithm(ByteContainer input_ByteContainer, ByteContainer[] round_keys)
        {
            ByteContainer output_ByteContainer = new ByteContainer(input_ByteContainer.get_bits());
            output_ByteContainer.initial_permutaion();
            Tuple<HalfByteContainer, HalfByteContainer> half_ByteContainers = output_ByteContainer.split_ByteContainer();
            HalfByteContainer left = half_ByteContainers.Item1;
            HalfByteContainer right = half_ByteContainers.Item2;
            for (int i = 0; i < round_keys.Length; i++)
            {
                bool[] temp = left.get_bits();
                left.set_bits(right.get_bits());
				//Console.WriteLine("right: " + right.as_binary_string());
                right.set_bits(CipherHandler.execute_cipher(right, round_keys[i]));
				//Console.WriteLine("right: " + right.as_binary_string());
                right.xor_bits(temp);
				//Console.WriteLine("right: " + right.as_binary_string());
            }
            output_ByteContainer.final_permutaion(left, right);
            return output_ByteContainer;
        }
    }

    public static class CipherHandler
    {
        public static bool[] execute_cipher(HalfByteContainer right_bits, ByteContainer round_key)
        {
			//First the right_bits are expanded into a byte
			ByteContainer s_box_input = right_bits.expand_bits();
			//The resulting byte then is xored with the round key.
			s_box_input.xor_bits(round_key.get_bits());
			//the xored byte is then passed to the s boxes which compress it into a mutated half byte.
            HalfByteContainer modified_half_ByteContainer = new HalfByteContainer(s_box.compression(s_box_input));
			//finally the half byte is permuted before being returned.
			modified_half_ByteContainer.permute_bits();
			return modified_half_ByteContainer.get_bits();
        }
    }

    public static class RoundKeyHandler
    {
        public static ByteContainer[] generate_round_keys(TenBitKey input, bool is_plaintext = true)
        {
			//Ten bit keys are split into the c and d half key blocks using the permuted choice 1 table.
            Tuple<HalfKey, HalfKey> half_keys = input.split_key();
			// variables for c and d are assigned to improved conciseness.
            HalfKey c = half_keys.Item1;
            HalfKey d = half_keys.Item2;
			//an array of 4 round keys are created.
            ByteContainer[] round_keys = gen_keys(c, d);
			//if the text is being decrypted the round keys are reversed.
            if (is_plaintext == false)
            {
                ByteContainer[] reverse_round_keys = new ByteContainer[] { round_keys[3], round_keys[2], round_keys[1], round_keys[0] };
                return reverse_round_keys;
            }
            return round_keys;
        }

        private static ByteContainer[] gen_keys(HalfKey c, HalfKey d)
        {
            ByteContainer[] round_keys = new ByteContainer[4];
            //the first round c and d are rotated 1 time.
			int num_rotations = 1;
            for (int i = 0; i < round_keys.Length; i++)
            {
                c.rotate_bits(num_rotations);
                d.rotate_bits(num_rotations);
				//the round key for the current round gets created using the permuted choice 2 table.
                round_keys[i] = new ByteContainer(permuted_choice_2(c, d));
				//at the end of the first round the number of rotations gets increased to 2.
                if (num_rotations == 1)
                {
                    num_rotations++;
                }
                Console.WriteLine("Round key " + (i+1) + ": " + round_keys[i].as_binary_string());
            }
            return round_keys;
        }

		//creates an 8 bit array from c and d using the permuted choice 2 table.
        private static bool[] permuted_choice_2(HalfKey c, HalfKey d)
        {
            bool[] c_bits = c.get_bits();
            bool[] d_bits = d.get_bits();
            return new bool[] { d_bits[0], c_bits[2], d_bits[1], c_bits[3], d_bits[2], c_bits[4], d_bits[4], d_bits[3] };
        }
    }    
}
