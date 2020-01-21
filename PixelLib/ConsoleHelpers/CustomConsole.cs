using System;
using System.IO;
using System.Text;

namespace PixelLib.ConsoleHelpers
{
	/// <summary>
	/// A class that acts as a wrapper around <see cref="Console"/>, to allow for implementing custom console behaviour.
	/// Provides mirrors of most Properties and Methods in <see cref="Console"/>.
	/// Also provides a wrapper event for <see cref="Console.CancelKeyPress"/>.
	/// This class is not <see langword="abstract"/>, with all members being virtual instead, to allow subclasses to only override the implementations for what they want,
	/// and to allow consumers to use a <see cref="CustomConsole"/> that simply mimics normal <see cref="Console"/> behaviour.
	/// </summary>
	/// <remarks>
	/// *Some methods, such as the non-params versions of <see cref="Console.Write(string, object[])"/> and <see cref="Console.WriteLine(string, object[])"/> are omitted.
	/// *All of the single parameter versions of <see cref="Console.Write(object)"/> and <see cref="Console.WriteLine(object)"/> have been substituted with a single Generic method.
	/// </remarks>
	public class CustomConsole
	{
#pragma warning disable IDE1006 // Naming Styles
		public virtual Encoding InputEncoding { get { return Console.InputEncoding; } set { Console.InputEncoding = value; }}
		public virtual Encoding OutputEncoding { get { return Console.OutputEncoding; } set { Console.OutputEncoding = value; }}
		public virtual ConsoleColor BackgroundColour { get { return Console.BackgroundColor; } set { Console.BackgroundColor = value; }}
		public virtual ConsoleColor ForegroundColour { get { return Console.ForegroundColor; } set { Console.ForegroundColor = value; }}
		public virtual int BufferWidth { get { return Console.BufferWidth; } set { Console.BufferWidth = value; }}
		public virtual int BufferHeight { get { return Console.BufferHeight; } set { Console.BufferHeight = value; }}
		public virtual int WindowWidth { get { return Console.WindowWidth; } set { Console.WindowWidth = value; }}
		public virtual int WindowHeight { get { return Console.WindowHeight; } set { Console.WindowHeight = value; }}
		public virtual int WindowLeft { get { return Console.WindowLeft; } set { Console.WindowLeft = value; }}
		public virtual int WindowTop { get { return Console.WindowTop; } set { Console.WindowTop = value; }}
		public virtual int CursorLeft { get { return Console.CursorLeft; } set { Console.CursorLeft = value; }}
		public virtual int CursorTop { get { return Console.CursorTop; } set { Console.CursorTop = value; }}
		public virtual int CursorSize { get { return Console.CursorSize; } set { Console.CursorSize = value; }}
		public virtual bool CursorVisible { get { return Console.CursorVisible; } set { Console.CursorVisible = value; }}
		public virtual bool TreatControlCAsInput { get { return Console.TreatControlCAsInput; } set { Console.TreatControlCAsInput = value; }}
		public virtual string Title { get { return Console.Title; } set { Console.Title = value; }}

		public virtual TextReader In => Console.In;
		public virtual TextWriter Out => Console.Out;
		public virtual TextWriter Error => Console.Error;
		public virtual int LargestWindowWidth => Console.LargestWindowWidth;
		public virtual int LargestWindowHeight => Console.LargestWindowHeight;
		public virtual bool IsInputRedirected => Console.IsInputRedirected;
		public virtual bool IsOutputRedirected => Console.IsOutputRedirected;
		public virtual bool IsErrorRedirected => Console.IsErrorRedirected;
		public virtual bool KeyAvailable => Console.KeyAvailable;
		public virtual bool NumberLock => Console.NumberLock;
		public virtual bool CapsLock => Console.CapsLock;
		
		/// <summary>
		/// Invoked whenever the <see cref="Console.CancelKeyPress"/> event is invoked, by default.
		/// </summary>
		public virtual event ConsoleCancelEventHandler CancelKeyPress;

		/// <summary>
		/// Invoked just before <see cref="Write{T}(T)"/>, or any of its overloads, gets called.
		/// </summary>
		public virtual event EventHandler preWriteEvent;

		/// <summary>
		/// Invoked just after <see cref="Write{T}(T)"/>, or any of its overloads, returns.
		/// </summary>
		public virtual event EventHandler postWriteEvent;

		/// <summary>
		/// Invoked just before <see cref="WriteLine"/>, or any of its overloads, gets called.
		/// </summary>
		public virtual event EventHandler preWriteLineEvent;

		/// <summary>
		/// Invoked just after <see cref="WriteLine"/>, or any of its overloads, returns.
		/// </summary>
		public virtual event EventHandler postWriteLineEvent;

		/// <summary>
		/// Invoked just before <see cref="Read"/>, <see cref="ReadKey(bool)"/> or <see cref="ReadLine"/> gets called.
		/// </summary>
		public virtual event EventHandler preReadEvent;

		/// <summary>
		/// Invoked just after <see cref="Read"/>, <see cref="ReadKey(bool)"/> or <see cref="ReadLine"/> stops blocking and returns.
		/// </summary>
		public virtual event EventHandler postReadEvent;

		public CustomConsole () { Console.CancelKeyPress += onRaiseConsoleCancelKeyPress; }

#pragma warning disable IDE0022 // Use block body for methods
		protected virtual void onRaiseConsoleCancelKeyPress (object sender, ConsoleCancelEventArgs e) => CancelKeyPress?.Invoke (sender, e);
		protected virtual void onPreWriteEvent () => preWriteEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPostWriteEvent () => postWriteEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPreWriteLineEvent () => preWriteLineEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPostWriteLineEvent () => postWriteLineEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPreReadEvent () => preReadEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPostReadEvent () => postReadEvent?.Invoke (this, EventArgs.Empty);

		public virtual Stream OpenStandardError () => Console.OpenStandardError ();
		public virtual Stream OpenStandardError (int bufferSize) => Console.OpenStandardError (bufferSize);
		public virtual Stream OpenStandardInput () => Console.OpenStandardInput ();
		public virtual Stream OpenStandardInput (int bufferSize) => Console.OpenStandardInput (bufferSize);
		public virtual Stream OpenStandardOutput () => Console.OpenStandardOutput ();
		public virtual Stream OpenStandardOutput (int bufferSize) => Console.OpenStandardOutput (bufferSize);

		public virtual void Beep () => Console.Beep ();
		public virtual void Beep (int frequency, int duration) => Console.Beep (frequency, duration);
		public virtual void Clear () => Console.Clear ();
		public virtual void ResetColour () => Console.ResetColor ();
		public virtual void SetBufferSize (int width, int height) => Console.SetBufferSize (width, height);
		public virtual void SetCursorPosition (int left, int top) => Console.SetCursorPosition (left, top);
		public virtual void SetError (TextWriter newError) => Console.SetError (newError);
		public virtual void SetIn (TextReader newIn) => Console.SetIn (newIn);
		public virtual void SetOut (TextWriter newOut) => Console.SetOut (newOut);
		public virtual void SetWindowPosition (int left, int top) => Console.SetWindowPosition (left, top);
		public virtual void SetWindowSize (int width, int height) => Console.SetWindowSize (width, height);

		public virtual void Write<T> (T value) { onPreWriteEvent (); Console.Write (value); onPostWriteEvent (); }
		public virtual void Write (char [] buffer) { onPreWriteEvent (); Console.Write (buffer); onPostWriteEvent (); }
		public virtual void Write (char [] buffer, int index, int count) { onPreWriteEvent (); Console.Write (buffer, index, count); onPostWriteEvent (); }
		public virtual void Write (string format, params object [] arg) { onPreWriteEvent (); Console.Write (format, arg); onPostWriteEvent (); }

		public virtual void WriteLine () { onPreWriteLineEvent (); Console.WriteLine (); onPostWriteLineEvent (); }
		public virtual void WriteLine<T> (T value) { onPreWriteLineEvent (); Console.WriteLine (value); onPostWriteLineEvent (); }
		public virtual void WriteLine (char [] buffer) { onPreWriteLineEvent (); Console.WriteLine (buffer); onPostWriteLineEvent (); }
		public virtual void WriteLine (char [] buffer, int index, int count) { onPreWriteLineEvent (); Console.WriteLine (buffer, index, count); onPostWriteLineEvent (); }
		public virtual void WriteLine (string format, params object [] arg) { onPreWriteLineEvent (); Console.WriteLine (format, arg); onPostWriteLineEvent (); }

		public virtual void MoveBufferArea (int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
			=> Console.MoveBufferArea (sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);

		public virtual void MoveBufferArea (int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColour, ConsoleColor sourceBackColour)
			=> Console.MoveBufferArea (sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColour, sourceBackColour);
#pragma warning restore IDE0022 // Use block body for methods

		public virtual int Read ()
		{
			onPreReadEvent ();
			int result = Console.Read ();
			onPostReadEvent ();
			return result;
		}

		public virtual ConsoleKeyInfo ReadKey ()
		{
			return ReadKey (false);
		}

		public virtual ConsoleKeyInfo ReadKey (bool intercept)
		{
			onPreReadEvent ();
			ConsoleKeyInfo result = Console.ReadKey (intercept);
			onPostReadEvent ();
			return result;
		}

		public virtual string ReadLine ()
		{
			onPreReadEvent ();
			string result = Console.ReadLine ();
			onPostReadEvent ();
			return result;
		}
#pragma warning restore IDE1006 // Naming Styles
	}
}
