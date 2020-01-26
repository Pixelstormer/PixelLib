using System;

namespace PixelLib.ExtensionMethods
{
	/// <summary>
	/// A set of extension methods to the <see cref="string"/> class.
	/// </summary>
	public static class StringExtensionMethods
	{
		/// <summary>
		/// Shuffles this <see cref="string"/>, randomising the positions of each <see cref="char"/>. Uses a <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates shuffle</a>.
		/// </summary>
		/// <seealso><a href="https://stackoverflow.com/a/4740014">Original source.</a></seealso>
		/// <param name="toShuffle">The given <see cref="string"/> to be shuffled.</param>
		/// <param name="random">The given <see cref="Random"/> to use.</param>
		/// <returns>A copy of <paramref name="toShuffle"/>, with the positions of each <see cref="char"/> randomised using a <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates shuffle</a>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="toShuffle"/> or <paramref name="random"/> are <see langword="null"/>.</exception>
		public static string shuffle (this string toShuffle, Random random)
		{
			if (toShuffle == null)
				throw new ArgumentNullException (nameof (toShuffle), $"Could not call method {nameof (shuffle)}: String object reference was null.");

			if (random == null)
				throw new ArgumentNullException (nameof (random), $"Must provide a valid {nameof (Random)} instance.");

			char [] array = toShuffle.ToCharArray ();

			int n = array.Length;
			while (n-- > 1)
			{
				int k = random.Next (n + 1);
				char value = array [k];
				array [k] = array [n];
				array [n] = value;
			}

			return new string (array);
		}

		/// <summary>
		/// Generates a <see cref="string"/> of length <paramref name="length"/>, composed of random <see cref="char"/>s from <paramref name="toSlice"/>.
		/// </summary>
		/// <remarks>If <paramref name="length"/> is equal to <c>0</c>, returns an empty <see cref="string"/>.</remarks>
		/// <param name="toSlice">The given <see cref="string"/> to select <see cref="char"/>s from.</param>
		/// <param name="random">The given <see cref="Random"/> instance to use.</param>
		/// <param name="length">The length of the result.</param>
		/// <returns>A string of length <paramref name="length"/>, consisting of randomly selected <see cref="char"/>s from <paramref name="toSlice"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="toSlice"/> or <paramref name="random"/> are <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="length"/> is less then <c>0</c></exception>
		public static string randomSlice (this string toSlice, Random random, int length)
		{
			if (toSlice == null)
				throw new ArgumentNullException (nameof (toSlice), $"Could not call method {nameof (randomSlice)}: String object reference was null.");

			if (random == null)
				throw new ArgumentNullException (nameof (random), $"Must provide a valid {nameof (Random)} instance.");

			if (length < 0)
				throw new ArgumentOutOfRangeException (nameof (length), length, $"{nameof (length)} must not be negative!");

			string result = "";

			while (result.Length < length)
				result += toSlice [random.Next (toSlice.Length)];

			return result;
		}

		/// <summary>
		/// Generates a <see cref="string"/> of the same <see cref="string.Length"/> as <paramref name="toSlice"/>, composed of random <see cref="char"/>s from <paramref name="toSlice"/>.
		/// </summary>
		/// <param name="toSlice">The given <see cref="string"/> used to specify the <see cref="string.Length"/> of the output, and to select <see cref="char"/>s from.</param>
		/// <param name="random">The given <see cref="Random"/> instance to use.</param>
		/// <returns>A string of the same <see cref="string.Length"/> as <paramref name="toSlice"/>, consisting of randomly selected <see cref="char"/>s from <paramref name="toSlice"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="toSlice"/> or <paramref name="random"/> are <see langword="null"/>.</exception>
		public static string randomSlice (this string toSlice, Random random)
		{
			if (toSlice == null)
				throw new ArgumentNullException (nameof (toSlice), $"Could not call method {nameof (randomSlice)}: String object reference was null.");

			return randomSlice (toSlice, random, toSlice.Length);
		}
	}
}
