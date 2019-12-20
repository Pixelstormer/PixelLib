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

		public PreConsoleInputEventArgs (CustomConsole consoleUsed)
		{
			this.consoleUsed = consoleUsed ?? throw new ArgumentNullException (nameof (consoleUsed), $"Cannot have a null {nameof (consoleUsed)}.");
		}
	}
}
