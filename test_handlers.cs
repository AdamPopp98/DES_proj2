using System;


namespace DesProject
{
	public static class AttackHandler
	{
		public static Tuple<TenBitKey, TenBitKey> BruteForceAttack(string input_string, string output_string, int search_size = 1024)
		{
			ByteContainer[] input_ByteContainers = FormatConverter.hex_to_ByteContainer_array(input_string);
			ByteContainer[] expected_output_ByteContainers = FormatConverter.hex_to_ByteContainer_array(output_string);
			TenBitKey key1 = new TenBitKey();
			int counter2 = 0;
			while (counter2 < 1024)
			{
				Console.WriteLine("Searching...");				
				int counter = 0;
				TenBitKey key2 = new TenBitKey();
				ByteContainer[] middle_cipher = CryptHandler.get_output_ecb(input_ByteContainers, key1);
				while (counter < 1024)
				{
					//Console.WriteLine("Checking key1: " + counter2 + " key2: " + counter + ".");
					ByteContainer[] check_against = CryptHandler.get_output_ecb(middle_cipher, key2);
					bool found_key = true;
					for (int i = 0; i < input_ByteContainers.Length; i++)
					{
						Console.WriteLine("key1: " + key1.as_binary_string() + " key2: " + key2.as_binary_string() + " output: " + FormatConverter.ByteContainer_array_to_hex(check_against));
						if (check_against[i].as_binary_string() != expected_output_ByteContainers[i].as_binary_string())
						{
							found_key = false;
							break;
						}
					}
					if (found_key == true)
					{
						Console.WriteLine("Key found");
						return Tuple.Create(key1, key2);
					}
					counter++;
					key2.increment_bits();
				}
				counter2++;
				key1.increment_bits();
			}
			Console.WriteLine("Key not found");			
			return Tuple.Create(new TenBitKey(), new TenBitKey());
		}

		public static Tuple<TenBitKey, TenBitKey> MeetInMiddleAttack(string plaintext, string ciphertext)
		{
			//converts strings to ByteContainer arrays at the start to avoid work duplication.
			ByteContainer[] input = FormatConverter.hex_to_ByteContainer_array(plaintext);
			ByteContainer[] output = FormatConverter.hex_to_ByteContainer_array(ciphertext);
			string[] middle_ciphers = new string[1024];
			string[] middle_ciphers2 = new string[1024];
			TenBitKey key1 = new TenBitKey();
			TenBitKey key2 = new TenBitKey();
			//the table of the intermediate values is constructed
			for (int i = 0; i < middle_ciphers.Length; i++)
			{
				ByteContainer[] cur = CryptHandler.get_output_ecb(input, key1);
				string cur_string = "";
				for (int j = 0; j < cur.Length; j++)
				{
					cur_string += cur[j].as_binary_string();
				}
				middle_ciphers[i] = cur_string;
				key1.increment_bits();
			}
			for (int i = 0; i < middle_ciphers2.Length; i++)
			{
				ByteContainer[] cur = CryptHandler.get_output_ecb(output, key2, false);
				string cur_string = "";
				for (int j = 0; j < cur.Length; j++)
				{
					cur_string += cur[j].as_binary_string();
				}
				middle_ciphers2[i] = cur_string;
				key2.increment_bits();
			}

			key1.set_bits("0000000000");
			for (int i = 0; i < middle_ciphers.Length; i++)
			{
				key2.set_bits("0000000000");
				for (int j = 0; j < middle_ciphers2.Length; j++)
				{
					if (middle_ciphers[i] == middle_ciphers2[j])
					{
						Console.WriteLine("key found");
						return Tuple.Create(key1, key2);
					}
					key2.increment_bits();
				}
				key1.increment_bits();
			}
			Console.WriteLine("key not found");
			return Tuple.Create(new TenBitKey(), new TenBitKey());
		}
	}
}