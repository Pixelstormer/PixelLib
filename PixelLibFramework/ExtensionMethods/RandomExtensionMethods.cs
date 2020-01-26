using System;

namespace PixelLib.ExtensionMethods
{
	/// <summary>
	/// A set of extension methods to the <see cref="Random"/> class, to allow for generating randomised <see cref="long"/> values.
	/// </summary>
	/// <seealso><a href="https://stackoverflow.com/a/13095144">Original source.</a></seealso>
	public static class RandomExtensionMethods
	{
		/// <summary>
		/// Generates a random <see cref="long"/>, from <paramref name="min"/> (Inclusive) to <paramref name="max"/> (Exclusive).
		/// </summary>
		/// <seealso cref="Random.Next(int, int)"/>
		/// <param name="random">The given random instance.</param>
		/// <param name="min">The inclusive minimum bound.</param>
		/// <param name="max">The exclusive maximum bound. Must be greater than <paramref name="min"/>.</param>
		/// <returns>A random <see cref="long"/>, between <paramref name="min"/> (Inclusive), and <paramref name="max"/> (Exclusive).</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="max"/> is less than or equal to <paramref name="min"/>.</exception>
#pragma warning disable IDE1006 // Naming Styles, allowing first letter of method names to be Upper Case to be consistent with the rest of Random's method names.
		public static long NextLong (this Random random, long min, long max)
		{
			if (random == null)
				throw new ArgumentNullException (nameof (random), $"Could not call method {nameof (NextLong)}: {nameof (Random)} object reference was null.");

			if (max <= min)
				throw new ArgumentOutOfRangeException (nameof (max), $"{nameof (max)} must be greater than {nameof (min)}!");

			// Working with ulong so that modulo works correctly with values > long.MaxValue.
			ulong uRange = (ulong) (max - min);

			// Prevent a modolo bias; see https://stackoverflow.com/a/10984975
			// for more information.
			// In the worst case, the expected number of calls is 2 (though usually it's
			// much closer to 1) so this loop doesn't really hurt performance at all.
			ulong ulongRand;
			byte[] buf = new byte[8];
			do
			{
				random.NextBytes (buf);
				ulongRand = (ulong) BitConverter.ToInt64 (buf, 0);
			} while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

			return (long) (ulongRand % uRange) + min;
		}

		/// <summary>
		/// Generates a random <see cref="long"/>, from <c>0</c> (Inclusive) to <paramref name="max"/> (Exclusive).
		/// </summary>
		/// <seealso cref="Random.Next(int)"/>
		/// <param name="random">The given random instance.</param>
		/// <param name="max">The exclusive maximum bound. Must be greater than <c>0</c>.</param>
		/// <returns>A random <see cref="long"/>, between <c>0</c> (Inclusive), and <paramref name="max"/> (Exclusive).</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="max"/> is less than or equal to <c>0</c>.</exception>
		public static long NextLong (this Random random, long max)
		{
			return NextLong (random, 0, max);
		}

		/// <summary>
		/// Generates a random <see cref="long"/>, from <see cref="long.MinValue"/> (Inclusive) to <see cref="long.MaxValue"/> (Exclusive).
		/// </summary>
		/// <seealso cref="Random.Next"/>
		/// <param name="random">The given random instance.</param>
		/// <returns>A random <see cref="long"/>, between <see cref="long.MinValue"/> (Inclusive), and <see cref="long.MaxValue"/> (Exclusive).</returns>
		public static long NextLong (this Random random)
		{
			return NextLong (random, long.MinValue, long.MaxValue);
		}
#pragma warning restore IDE1006 // Naming Styles
	}
}
