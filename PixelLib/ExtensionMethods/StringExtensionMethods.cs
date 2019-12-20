using System;

namespace PixelLib.ExtensionMethods
{
	/// <summary>
	/// A set of extension methods to the <see cref="string"/> class, to allow for randomised manipulation.
	/// </summary>
	public static class StringExtensionMethods
	{
		/// <summary>
		/// Shuffles this <see cref="string"/>, randomising the positions of each <see cref="char"/>. Uses a <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates shuffle</a>.
		/// </summary>
		/// <seealso><a href="https://stackoverflow.com/a/4740014">Original source.</a></seealso>
		/// <param name="str">The given <see cref="string"/> to be shuffled.</param>
		/// <param name="random">The given <see cref="Random"/> to use.</param>
		/// <returns>A copy of <paramref name="str"/>, with the positions of each <see cref="char"/> randomised using a <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates shuffle</a>.</returns>
		public static string shuffle (this string str, Random random)
		{
			char[] array = str.ToCharArray ();

			int n = array.Length;
			while (n-- > 1)
			{
				int k = random.Next (n + 1);
				char value = array[k];
				array[k] = array[n];
				array[n] = value;
			}

			return new string (array);
		}

		/// <summary>
		/// Generates a <see cref="string"/> of length <paramref name="length"/>, composed of random <see cref="char"/>s from <paramref name="str"/>.
		/// </summary>
		/// <remarks>If <paramref name="length"/> is equal to <c>0</c>, returns an empty <see cref="string"/>.</remarks>
		/// <param name="str">The given <see cref="string"/> to select <see cref="char"/>s from.</param>
		/// <param name="random">The given <see cref="Random"/> instance to use.</param>
		/// <param name="length">The length of the result.</param>
		/// <returns>A string of length <paramref name="length"/>, consisting of randomly selected <see cref="char"/>s from <paramref name="str"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="length"/> is less then <c>0</c></exception>
		public static string randomSlice (this string str, Random random, int length)
		{
			if (length < 0)
				throw new ArgumentOutOfRangeException (nameof (length), length, $"{nameof (length)} must not be negative!");

			string result = "";

			while (result.Length < length)
				result += str[random.Next (str.Length)];

			return result;
		}

		/// <summary>
		/// Generates a <see cref="string"/> of the same <see cref="string.Length"/> as <paramref name="str"/>, composed of random <see cref="char"/>s from <paramref name="str"/>.
		/// </summary>
		/// <param name="str">The given <see cref="string"/> used to specify the <see cref="string.Length"/> of the output, and to select <see cref="char"/>s from.</param>
		/// <param name="random">The given <see cref="Random"/> instance to use.</param>
		/// <returns>A string of the same <see cref="string.Length"/> as <paramref name="str"/>, consisting of randomly selected <see cref="char"/>s from <paramref name="str"/>.</returns>
		public static string randomSlice (this string str, Random random)
		{
			return randomSlice (str, random, str.Length);
		}
	}
}
