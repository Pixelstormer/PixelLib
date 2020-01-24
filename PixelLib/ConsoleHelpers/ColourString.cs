using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelLib.ConsoleHelpers
{
	/// <summary>
	/// Represents a <see cref="string"/> that has a foreground and background <see cref="ConsoleColor"/> associated with it.
	/// </summary>
	// As this class acts as a string wrapper, suppressing CA1710 is fine because the intent is clear from the current identifier.
	[System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public struct ColourString : IEquatable<ColourString>, IComparable<ColourString>, IEnumerable<char>
	{
		/// <summary>
		/// The foreground <see cref="ConsoleColor"/> associated with this <see cref="ColourString"/>.
		/// </summary>
		public ConsoleColor foregroundColour { get; }

		/// <summary>
		/// The background <see cref="ConsoleColor"/> associated with this <see cref="ColourString"/>.
		/// </summary>
		public ConsoleColor backgroundColour { get; }

		/// <summary>
		/// The <see cref="string"/> this <see cref="ColourString"/> represents.
		/// </summary>
		public string text { get; }

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
			return string.Compare (text, other.text, StringComparison.Ordinal);
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
		/// Compares this <see cref="ColourString"/> with <paramref name="obj"/> for equality.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to compare this to.</param>
		/// <returns>Whether or not this <see cref="ColourString"/> and <paramref name="obj"/> are equal.</returns>
		public override bool Equals (object obj)
		{
			if (obj is ColourString)
				return Equals ((ColourString) obj);
			return false;
		}

		/// <summary>
		/// Gets the hash code for this <see cref="ColourString"/>.
		/// </summary>
		/// <returns>The hash code for this <see cref="ColourString"/>.</returns>
		public override int GetHashCode ()
		{
			int hashCode = -1917421105;
			hashCode *= -1521134295 + foregroundColour.GetHashCode ();
			hashCode *= -1521134295 + backgroundColour.GetHashCode ();
			hashCode *= -1521134295 + text.GetHashCode ();
			return hashCode;
		}

		/// <summary>
		/// Compares two <see cref="ColourString"/>s for equality.
		/// </summary>
		/// <param name="first">The first <see cref="ColourString"/> to compare.</param>
		/// <param name="second">The second <see cref="ColourString"/> to compare.</param>
		/// <returns>Whether or not <paramref name="first"/> and <paramref name="second"/> are equal.</returns>
		public static bool operator == (ColourString first, ColourString second) => first.Equals (second);

		/// <summary>
		/// Compares two <see cref="ColourString"/>s for inequality.
		/// </summary>
		/// <param name="first">The first <see cref="ColourString"/> to compare.</param>
		/// <param name="second">The second <see cref="ColourString"/> to compare.</param>
		/// <returns>Whether or not <paramref name="first"/> and <paramref name="second"/> are unequal.</returns>
		public static bool operator != (ColourString first, ColourString second) => !first.Equals (second);

		public static bool operator < (ColourString first, ColourString second) => first.CompareTo (second) < 0;
		public static bool operator > (ColourString first, ColourString second) => first.CompareTo (second) > 0;

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
