using System;
using DesProject;


namespace DesProject {
	static class Program
	{
		static void Main()
		{
			/*HalfByteContainer test_half_ByteContainer = new HalfByteContainer();
			int counter = 0;
			while (counter < 16)
			{
				Tuple<int, int> actual = s_box.get_matrix_coords(test_half_ByteContainer);
				int matrix_val = s_box.get_matrix_val(actual, new int[,] {{1, 0, 3, 2}, {3, 2, 1, 0}, {0, 2, 1, 3}, {3, 1, 3, 2}});
				Tuple<bool, bool> as_bits = s_box.get_bit_vals(matrix_val);
				Console.WriteLine("input: " + test_half_ByteContainer.as_binary_string() + " row: " + actual.Item1 + " col: " + actual.Item2 + " s_box1 result: " + matrix_val + " bits = " + as_bits.Item1 + " " + as_bits.Item2);
				counter++;
				test_half_ByteContainer.increment_bits();
			}*/

			//test_half_ByteContainer.set_bits(new bool[] {false, false, false, false});
			TenBitKey test_key = new TenBitKey();
			
			Console.WriteLine("key: " + test_key.as_binary_string());
			ByteContainer[] ByteContainers = { new ByteContainer("80") };
			int counter = 0;
			while (counter < 8)
			{
				ByteContainer[] output_ByteContainers = new ByteContainer[1];
				output_ByteContainers = DesProject.CryptHandler.get_output_ecb(ByteContainers, test_key);
				Console.WriteLine("Plaintext: " + ByteContainers[0].as_binary_string() + " Ciphertext: " + output_ByteContainers[0].as_binary_string());
				ByteContainers[0].right_shift_bits();
				counter++;
			}

			TenBitKey test_key2 = new TenBitKey("1000000000");
			ByteContainer[] ByteContainers2 = { new ByteContainer("00") };
			Console.WriteLine("\nPlaintext: " + ByteContainers2[0].as_binary_string());
			counter = 0;
			while (counter < 10)
			{
				ByteContainer[] output_ByteContainers2 = new ByteContainer[1];
				output_ByteContainers2 = DesProject.CryptHandler.get_output_ecb(ByteContainers2, test_key2);
				Console.WriteLine("key: " + test_key2.as_binary_string() + " Ciphertext: " + output_ByteContainers2[0].as_binary_string());
				test_key2.right_shift_bits();
				counter++;
			}

			Console.WriteLine("\nPlaintext: " + ByteContainers2[0].as_binary_string());
			TenBitKey test_key3 = new TenBitKey("0000000011");
			ByteContainer[] output_ByteContainers3 = new ByteContainer[1];
			output_ByteContainers3 = DesProject.CryptHandler.get_output_ecb(ByteContainers2, test_key3);
			Console.WriteLine("key: " + test_key3.as_binary_string() + " Ciphertext: " + output_ByteContainers3[0].as_binary_string());
			test_key2.right_shift_bits();
			//Tuple<DesProject.TenBitKey, DesProject.TenBitKey> keys = DesProject.AttackHandler.BruteForceAttack("4272757465", "52f0be698a");
			Tuple<DesProject.TenBitKey, DesProject.TenBitKey> keys = DesProject.AttackHandler.MeetInMiddleAttack("4272757465", "52f0be698a");
		}
	}	
}
