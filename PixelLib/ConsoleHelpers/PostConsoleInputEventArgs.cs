using System;

namespace PixelLib.ConsoleHelpers
{
	/// <summary>
	/// Provides data for the <see cref="ConsoleInputListener.postConsoleInputEvent"/> event.
	/// </summary>
	public class PostConsoleInputEventArgs : EventArgs
	{
		/// <summary>
		/// The <see cref="CustomConsole"/> that was used by the <see cref="ConsoleInputListener"/> that generated this <see cref="PostConsoleInputEventArgs"/> instance.
		/// </summary>
		public CustomConsole consoleUsed { get; }

		/// <summary>
		/// The input received by the <see cref="ConsoleInputListener"/> that generated this <see cref="PostConsoleInputEventArgs"/> instance.
		/// </summary>
		public string consoleInput { get; }

		/// <summary>
		/// Indicates whether or not the <see cref="ConsoleInputListener"/> that generated this <see cref="PostConsoleInputEventArgs"/> instance should stop listening.
		/// </summary>
		public bool cancelRequested { get; set; }

		/// <summary>
		/// Creates a new <see cref="PostConsoleInputEventArgs"/>, with <paramref name="consoleUsed"/> as the referenced <see cref="CustomConsole"/>, and <paramref name="consoleInput"/> as the recorded text.
		/// </summary>
		/// <param name="consoleUsed">The <see cref="CustomConsole"/> to reference with <see cref="consoleUsed"/>.</param>
		/// <param name="consoleInput">The text recorded into <see cref="consoleInput"/></param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="consoleUsed"/> is <see langword="null"/></exception>
		public PostConsoleInputEventArgs (CustomConsole consoleUsed, string consoleInput)
		{
			if (consoleUsed == null)
				throw new ArgumentNullException (nameof (consoleUsed), $"Cannot have a null {nameof (consoleUsed)}.");
			this.consoleUsed = consoleUsed;
			this.consoleInput = consoleInput;
		}
	}
}
