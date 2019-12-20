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

		public PostConsoleInputEventArgs (CustomConsole consoleUsed, string consoleInput)
		{
			this.consoleUsed = consoleUsed ?? throw new ArgumentNullException (nameof (consoleUsed), $"Cannot have a null {nameof (consoleUsed)}.");
			this.consoleInput = consoleInput;
		}
	}
}
