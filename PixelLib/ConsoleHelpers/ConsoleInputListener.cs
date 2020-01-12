using System;

namespace PixelLib.ConsoleHelpers
{
	public class ConsoleInputListener
	{
		/// <summary>
		/// Raised just before <see cref="startListening"/> attempts to read input.
		/// </summary>
		public event EventHandler<PreConsoleInputEventArgs> preConsoleInputEvent;

		/// <summary>
		/// Raised just after <see cref="startListening"/> reads input.
		/// </summary>
		public event EventHandler<PostConsoleInputEventArgs> postConsoleInputEvent;

		private CustomConsole console;

		public ConsoleInputListener (CustomConsole console)
		{
			this.console = console;
		}

#pragma warning disable IDE0022 // Use block body for methods
		protected virtual void onPreConsoleInputEvent (PreConsoleInputEventArgs e) => preConsoleInputEvent?.Invoke (this, e);
		protected virtual void onPostConsoleInputEvent (PostConsoleInputEventArgs e) => postConsoleInputEvent?.Invoke (this, e);
#pragma warning restore IDE0022 // Use block body for methods

		/// <summary>
		/// Start repeatedly listening for console input. Uses <see cref="CustomConsole.ReadLine"/>,
		/// and blocks indefinitely until a subscriber of either the <see cref="preConsoleInputEvent"/> or <see cref="postConsoleInputEvent"/> requests a cancellation.
		/// </summary>
		public void startListening ()
		{
			while (true)
			{
				PreConsoleInputEventArgs preInputArgs = new PreConsoleInputEventArgs (console);
				onPreConsoleInputEvent (preInputArgs);

				if (preInputArgs.cancelRequested)
					break;

				string input = console.ReadLine ();

				PostConsoleInputEventArgs postInputArgs = new PostConsoleInputEventArgs (console, input);
				onPostConsoleInputEvent (postInputArgs);

				if (postInputArgs.cancelRequested)
					break;
			}
		}
	}
}
