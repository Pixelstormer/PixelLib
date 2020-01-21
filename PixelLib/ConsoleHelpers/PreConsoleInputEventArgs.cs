using System;

namespace PixelLib.ConsoleHelpers
{
	/// <summary>
	/// Provides data for the <see cref="ConsoleInputListener.preConsoleInputEvent"/> event.
	/// </summary>
	public class PreConsoleInputEventArgs : EventArgs
	{
		/// <summary>
		/// The <see cref="CustomConsole"/> that was used by the <see cref="ConsoleInputListener"/> that generated this <see cref="PreConsoleInputEventArgs"/> instance.
		/// </summary>
		public CustomConsole consoleUsed { get; }

		/// <summary>
		/// Indicates whether or not the <see cref="ConsoleInputListener"/> that generated this <see cref="PreConsoleInputEventArgs"/> instance should stop listening before attempting to read input.
		/// </summary>
		public bool cancelRequested { get; set; }

		/// <summary>
		/// Creates a new <see cref="PreConsoleInputEventArgs"/>, with <paramref name="consoleUsed"/> as the referenced <see cref="CustomConsole"/>.
		/// </summary>
		/// <param name="consoleUsed">The <see cref="CustomConsole"/> to reference with <see cref="consoleUsed"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="consoleUsed"/> is <see langword="null"/>.</exception>
		public PreConsoleInputEventArgs (CustomConsole consoleUsed)
		{
			if (consoleUsed == null)
				throw new ArgumentNullException (nameof (consoleUsed), $"Cannot have a null {nameof (consoleUsed)}.");
			this.consoleUsed = consoleUsed;
		}
	}
}
