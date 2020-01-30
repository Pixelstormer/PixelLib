using System;

namespace PixelLib.ExtensionMethods
{
	/// <summary>
	/// A set of extension methods for Arrays.
	/// </summary>
	public static class ArrayExtensionMethods
	{
		/// <summary>
		/// Checks whether <paramref name="array"/> contains <paramref name="target"/>.
		/// </summary>
		/// <seealso cref="object.Equals(object, object)"/>
		/// <typeparam name="T">The type stored by <paramref name="array"/>.</typeparam>
		/// <param name="array">The given array to check for the presence of <paramref name="target"/> in.</param>
		/// <param name="target">The given element to check for.</param>
		/// <returns>Whether or not <see cref="object.Equals(object, object)"/> returns <see langword="true"/>, for <paramref name="target"/> and at least one element of <paramref name="array"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <see langword="null"/>.</exception>
		public static bool contains<T> (this T [] array, T target)
		{
			if (array == null)
				throw new ArgumentNullException (nameof (array), $"Could not call method {nameof (contains)}: Array object reference was null.");

			foreach (T item in array)
				if (Equals (target, item))
					return true;
			return false;
		}

		/// <summary>
		/// Checks whether or not <paramref name="array"/> contains an object reference to <paramref name="target"/>.
		/// </summary>
		/// <remarks>Due to the nature of <see cref="object.ReferenceEquals(object, object)"/>, this method may be unreliable when used with <see cref="ValueType"/>s or <see cref="string"/>s.</remarks>
		/// <seealso cref="object.ReferenceEquals(object, object)"/>
		/// <typeparam name="T">The type stored by <paramref name="array"/>.</typeparam>
		/// <param name="array">The given array to check for an object reference to <paramref name="target"/> in.</param>
		/// <param name="target">The given element to check for an object reference to.</param>
		/// <returns>Whether or not <see cref="object.ReferenceEquals(object, object)"/> returns <see langword="true"/>, for <paramref name="target"/> and at least one element of <paramref name="array"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <see langword="null"/>.</exception>
		public static bool referenceContains<T> (this T [] array, T target)
		{
			if (array == null)
				throw new ArgumentNullException (nameof (array), $"Could not call method {nameof (referenceContains)}: Array object reference was null.");

			foreach (T item in array)
				if (ReferenceEquals (target, item))
					return true;
			return false;
		}

		/// <summary>
		/// Checks whether or not <paramref name="array"/> contains a <see cref="string"/> that is equal to <paramref name="target"/>,
		/// according to <paramref name="options"/>.
		/// </summary>
		/// <seealso cref="string.Equals(string, string, StringComparison)"/>
		/// <param name="array">The given array to check for the presence of <paramref name="target"/> in.</param>
		/// <param name="target">The given <see cref="string"/> to check for.</param>
		/// <param name="options">The given <see cref="StringComparison"/> options by which to compare <paramref name="target"/> with the elements in <paramref name="array"/>.</param>
		/// <returns>Whether or not <see cref="string.Equals(string, string, StringComparison)"/> returns <see langword="true"/>,
		/// for <paramref name="target"/> and at least one element of <paramref name="array"/>, according to <paramref name="options"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <see langword="null"/>.</exception>
		public static bool contains (this string [] array, string target, StringComparison options)
		{
			if (array == null)
				throw new ArgumentNullException (nameof (array), $"Could not call method {nameof (contains)}: Array object reference was null.");

			foreach (string item in array)
				if (string.Equals (target, item, options))
					return true;
			return false;
		}

		/// <summary>
		/// Attempts to cast all of the elements in <paramref name="array"/> to type <typeparamref name="T"/>, inserting the results into <paramref name="outArray"/>.
		/// </summary>
		/// <remarks>If this method returns <see langword="true"/>, <paramref name="outArray"/> will be filled with the elements of <paramref name="array"/>, cast to type <typeparamref name="T"/>.
		/// If <paramref name="outArray"/> has a greater length than <paramref name="array"/>, it will only be filled up to the length of <paramref name="array"/>; further elements will remain untouched.
		/// If this method returns <see langword="false"/>, <paramref name="outArray"/> will be set to a <see langword="null"/> reference.</remarks>
		/// <typeparam name="T">The type to attempt to cast to.</typeparam>
		/// <param name="array">The array of <see cref="object"/>s to attempt to cast the elements of.</param>
		/// <param name="outArray">If the method returns <see langword="true"/>, this array will be filled with the elements of <paramref name="array"/>, cast to type <typeparamref name="T"/>. Otherwise, it will be set to a <see langword="null"/> reference.</param>
		/// <returns>Whether or not every element in <paramref name="array"/> could be cast to <typeparamref name="T"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="outArray"/> has a smaller length than <paramref name="array"/>.</exception>
		public static bool cast<T> (this object [] array, ref T [] outArray)
		{
			if (array == null)
				throw new ArgumentNullException (nameof (array), "Cannot cast the contents of a null array.");

			outArray ??= new T [array.Length];

			if (outArray.Length < array.Length)
				throw new ArgumentException ("The output array cannot be smaller than the input array.", nameof (outArray));

			for (int i = 0; i < array.Length; ++i)
			{
				if (array [i] is T)
					outArray [i] = (T) array [i];
				else
				{
					outArray = null;
					return false;
				}
			}
			
			return true;
		}
	}
}
