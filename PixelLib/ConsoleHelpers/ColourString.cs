using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelLib.ConsoleHelpers
{
	/// <summary>
	/// Represents a <see cref="string"/> that has a foreground and background <see cref="ConsoleColor"/> associated with it.
	/// </summary>
	public struct ColourString : IEquatable<ColourString>, IComparable<ColourString>, IEnumerable<char>
	{
		/// <summary>
		/// The foreground <see cref="ConsoleColor"/> associated with this <see cref="ColourString"/>.
		/// </summary>
		public readonly ConsoleColor foregroundColour;

		/// <summary>
		/// The background <see cref="ConsoleColor"/> associated with this <see cref="ColourString"/>.
		/// </summary>
		public readonly ConsoleColor backgroundColour;

		/// <summary>
		/// The <see cref="string"/> this <see cref="ColourString"/> represents.
		/// </summary>
		public readonly string text;

		/// <summary>
		/// Creates a new <see cref="ColourString"/> with the specified <paramref name="foregroundColour"/>, <paramref name="backgroundColour"/> and <paramref name="text"/>.
		/// </summary>
		/// <param name="foregroundColour">The foreground <see cref="ConsoleColor"/> associated with this <see cref="ColourString"/>.</param>
		/// <param name="backgroundColour">The background <see cref="ConsoleColor"/> associated with this <see cref="ColourString"/>.</param>
		/// <param name="text">The <see cref="string"/> that this <see cref="ColourString"/> represents.</param>
		public ColourString (ConsoleColor foregroundColour, ConsoleColor backgroundColour, string text)
		{
			this.foregroundColour = foregroundColour;
			this.backgroundColour = backgroundColour;
			this.text = text;
		}

		/// <summary>
		/// Compares this <see cref="ColourString"/> with <paramref name="other"/>, equivalent to the <see cref="string.Compare(string, string)"/> of each <see cref="text"/>.
		/// </summary>
		/// <param name="other">The <see cref="ColourString"/> to compare this <see cref="ColourString"/> to.</param>
		/// <returns>The value of <see cref="string.Compare(string, string)"/> with the <see cref="text"/> of this and <paramref name="other"/>.</returns>
		public int CompareTo (ColourString other)
		{
			return string.Compare (text, other.text);
		}

		/// <summary>
		/// Compares this <see cref="ColourString"/> with <paramref name="other"/> for equality.
		/// </summary>
		/// <param name="other">The <see cref="ColourString"/> to compare this to.</param>
		/// <returns>Whether or not this <see cref="ColourString"/> and <paramref name="other"/> are equal.</returns>
		public bool Equals (ColourString other)
		{
			return foregroundColour == other.foregroundColour
				&& backgroundColour == other.backgroundColour
				&& string.Equals (text, other.text, StringComparison.Ordinal);
		}

		/// <summary>
		/// Get an enumerator over the <see cref="char"/>s in <see cref="text"/>.
		/// </summary>
		/// <returns>An enumerator over the <see cref="char"/>s in <see cref="text"/>.</returns>
		public IEnumerator<char> GetEnumerator ()
		{
			return text.GetEnumerator ();
		}

		/// <summary>
		/// Get an enumerator over the <see cref="char"/>s in <see cref="text"/>.
		/// </summary>
		/// <returns>An enumerator over the <see cref="char"/>s in <see cref="text"/>.</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return text.GetEnumerator ();
		}
	}
}
