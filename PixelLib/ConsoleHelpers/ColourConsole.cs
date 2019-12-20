using System;
using System.Collections.Generic;

namespace PixelLib.ConsoleHelpers
{
	/// <summary>
	/// Allows for far finer control of <see cref="BackgroundColour"/> and <see cref="ForegroundColour"/> settings.
	/// </summary>
	public class ColourConsole : CustomConsole
	{
		public const char STRINGFORMAT_ESCAPE = '\\';
		public const char STRINGFORMAT_STARTBLOCK = '{';
		public const char STRINGFORMAT_ENDBLOCK = '}';
		public const char STRINGFORMAT_COLOURSEP = ':';

		private const ConsoleColor DEFAULT_BACKGROUNDCOLOUR = ConsoleColor.Black;
		private const ConsoleColor DEFAULT_FOREGROUNDCOLOUR = ConsoleColor.White;

		private ConsoleColor defaultBackgroundColour { get; }
		private ConsoleColor defaultForegroundColour { get; }

		private ConsoleColor previousBackgroundColour { get; set; }
		private ConsoleColor previousForegroundColour { get; set; }

		private bool isStaged;

		/// <summary>
		/// The <see cref="ConsoleColor"/> of any text printed by this wrapper.
		/// </summary>
		public override ConsoleColor BackgroundColour { get; set; }

		/// <summary>
		/// The <see cref="ConsoleColor"/> of any text printed by this wrapper.
		/// </summary>
		public override ConsoleColor ForegroundColour { get; set; }

		/// <summary>
		/// Instantiates a new wrapper, with <see cref="DEFAULT_BACKGROUNDCOLOUR"/> as the <see cref="BackgroundColour"/>,
		/// and <see cref="DEFAULT_FOREGROUNDCOLOUR"/> as the <see cref="ForegroundColour"/>.
		/// </summary>
		public ColourConsole ()
			: this (DEFAULT_BACKGROUNDCOLOUR, DEFAULT_FOREGROUNDCOLOUR) { }

		/// <summary>
		/// Instantiates a new wrapper, with <paramref name="backgroundColour"/> as the <see cref="BackgroundColour"/>,
		/// and <paramref name="foregroundColour"/> as the <see cref="ForegroundColour"/>.
		/// </summary>
		/// <param name="backgroundColour">The <see cref="BackgroundColour"/> this wrapper will use then printing text.</param>
		/// <param name="foregroundColour">The <see cref="ForegroundColour"/> this wrapper will use when printing text.</param>
		public ColourConsole (ConsoleColor foregroundColour, ConsoleColor backgroundColour)
		{
			previousBackgroundColour = base.BackgroundColour;
			previousForegroundColour = base.ForegroundColour;

			defaultBackgroundColour = backgroundColour;
			defaultForegroundColour = foregroundColour;

			BackgroundColour = backgroundColour;
			ForegroundColour = foregroundColour;

			PreWriteEvent += handlePreWriteOperation;
			PreWriteLineEvent += handlePreWriteOperation;

			PostWriteEvent += handlePostWriteOperation;
			PostWriteLineEvent += handlePostWriteOperation;
		}

		/// <summary>
		/// Sets the <see cref="CustomConsole"/>'s <see cref="CustomConsole.BackgroundColor"/> and <see cref="CustomConsole.ForegroundColor"/>
		/// to this wrapper's <see cref="BackgroundColour"/> and <see cref="ForegroundColour"/>,
		/// before clearing the <see cref="CustomConsole"/> with <see cref="CustomConsole.Clear"/>
		/// to set the <see cref="ConsoleColor"/> of the entire console window to this wrapper's <see cref="BackgroundColour"/>.
		/// </summary>
		public void clearConsole ()
		{
			base.BackgroundColour = BackgroundColour;
			base.ForegroundColour = ForegroundColour;
			Clear ();
		}

#pragma warning disable IDE0022 // Use block body for methods
		private void handlePreWriteOperation (object sender, EventArgs e) => stageColours (BackgroundColour, ForegroundColour);
		private void handlePostWriteOperation (object sender, EventArgs e) => unstageColours ();
#pragma warning restore IDE0022 // Use block body for methods

		private void stageColours (ConsoleColor foreground, ConsoleColor background)
		{
			if (isStaged)
				return;

			previousBackgroundColour = base.BackgroundColour;
			previousForegroundColour = base.ForegroundColour;
			base.BackgroundColour = background;
			base.ForegroundColour = foreground;
			isStaged = true;
		}

		private void unstageColours ()
		{
			base.BackgroundColour = previousBackgroundColour;
			base.ForegroundColour = previousForegroundColour;
			isStaged = false;
		}

		private ((ConsoleColor foreground, ConsoleColor background) colours, string text)[] formatString (string format, params ConsoleColor[] args)
		{
			// -- These notes are outdated and wrong, and do not represent how this function actually works. --
			// format -> "abc {0} XYZ" -> 'abc ' has current colours, ' XYZ' has params index 0 foreground, default background
			// format -> "{White} abc {:Black} xyz" -> ' abc ' has ConsoleColor.White foreground, current background. ' xyz' has current foreground, ConsoleColor.Black bckground
			// format -> "abc XYZ" -> current colours
			// format -> "{0:1} abc {2:0} xyz" -> ' abc ' has params index 0 foreground, params index 1 background. ' xyz' has params index 2 foreground, params index 0 background
			// format -> "{Black:White} abc" -> ' abc' has ConsoleColor.Black foreground, ConsoleColor.White background
			// format -> "\{0} abc" -> '\{0} abc' has current colours.
			// format -> "}\ abc{" -> '}\ abc{' has current colours.
			// format -> "\\{0} abc" -> '\{0} abc' has current colours.
			// ------------------------------------------------------------------------------------------------

			// If format doesn't specify a ConsoleColour to start with, default to the current colours.
			if (!format.StartsWith ("{"))
				format = $"{{{ForegroundColour.ToString ()}:{BackgroundColour.ToString ()}}}" + format;

			List<string> blocks = new List<string> (args.Length);
			List<(ConsoleColor foreground, ConsoleColor background)> blockColours = new List<(ConsoleColor foreground, ConsoleColor background)> (args.Length);

			bool inColourBlock = false;
			bool escapeNextChar = false;
			bool foundSeparator = false;

			string currentBlock = "";

			(ConsoleColor foreground, ConsoleColor background) currentColours = (ForegroundColour, BackgroundColour);

			for (int i = 0; i < format.Length; i++)
			{
				char currentChar = format[i];

				if (escapeNextChar)
				{
					currentBlock += currentChar;
					escapeNextChar = false;
				}
				else
				{
					switch (currentChar)
					{
						case STRINGFORMAT_ESCAPE:
							if (inColourBlock)
								throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Got {nameof (STRINGFORMAT_ESCAPE)} char ('{STRINGFORMAT_ESCAPE}') while in a block at index: {i}.");
							escapeNextChar = true;
							break;

						case STRINGFORMAT_STARTBLOCK:
							if (inColourBlock)
								throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Got {nameof (STRINGFORMAT_STARTBLOCK)} char ('{STRINGFORMAT_STARTBLOCK}') while already in a block at index: {i}.)");
							inColourBlock = true;
							if (i != 0)
								blocks.Add (currentBlock);
							currentBlock = "";
							break;

						case STRINGFORMAT_COLOURSEP:
							if (foundSeparator)
								throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Got additional {nameof (STRINGFORMAT_COLOURSEP)} char before reaching end of block at index {i}.)");

							if (inColourBlock)
							{
								if (string.IsNullOrWhiteSpace (currentBlock))
									currentColours.foreground = ForegroundColour;
								else if (int.TryParse (currentBlock, out int paramsIndex))
								{
									try
									{ currentColours.foreground = args[paramsIndex]; }
									catch (IndexOutOfRangeException e)
									{ throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Specified {nameof (args)} index '{paramsIndex}' is out of range at index {i}.)", e); }
								}
								else if (Enum.TryParse (currentBlock, true, out ConsoleColor colour))
									currentColours.foreground = colour;
								else
									throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Got invalid Colour Block: '{currentBlock}' at index: {i}.)");

								currentBlock = "";
								foundSeparator = true;
							}
							else
								currentBlock += currentChar;
							break;

						case STRINGFORMAT_ENDBLOCK:
							if (!inColourBlock)
								throw new FormatException ($"{nameof (formatString)} received bad format string: '{format}'. (Got {nameof (STRINGFORMAT_ENDBLOCK)} char ('{STRINGFORMAT_ENDBLOCK}') while not in a block at index: {i}.");

							if (!foundSeparator)
								throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Got {nameof (STRINGFORMAT_ENDBLOCK)} char ('{STRINGFORMAT_ENDBLOCK}') before finding a {nameof (STRINGFORMAT_COLOURSEP)} char ('{STRINGFORMAT_COLOURSEP}') at index {i}.)");

							if (string.IsNullOrWhiteSpace (currentBlock))
								currentColours.background = ForegroundColour;
							else if (int.TryParse (currentBlock, out int paramsIndex))
							{
								try
								{ currentColours.background = args[paramsIndex]; }
								catch (IndexOutOfRangeException e)
								{ throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Specified {nameof (args)} index '{paramsIndex}' is out of range at index {i}.)", e); }
							}
							else if (Enum.TryParse (currentBlock, true, out ConsoleColor colour))
								currentColours.background = colour;
							else
								throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Got invalid Colour Block: '{currentBlock}' at index: {i}.)");

							blockColours.Add (currentColours);
							currentBlock = "";
							inColourBlock = false;
							foundSeparator = false;
							break;

						default:
							currentBlock += currentChar;
							break;
					}
				}
			}

			if (inColourBlock)
				throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Reached end of string while still in block.)");

			if (string.IsNullOrEmpty (currentBlock))
				throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Block at end of string had no following chars.)");

			blocks.Add (currentBlock);

			if (blocks.Count != blockColours.Count)
				throw new FormatException ($"{nameof (formatString)} recieved bad format string: '{format}'. (Got unequal number of Colour Blocks ({blockColours.Count}) and Text Blocks ({blocks.Count}).)");

			((ConsoleColor foreground, ConsoleColor background) colours, string text)[] result = new ((ConsoleColor foreground, ConsoleColor background), string)[blocks.Count];

			for (int i = 0; i < blocks.Count; i++)
			{
				result[i].colours = blockColours[i];
				result[i].text = blocks[i];
			}

			return result;
		}

#pragma warning disable IDE1006 // Naming Styles
		public void Write (string format, params ConsoleColor[] args)
		{
			((ConsoleColor foreground, ConsoleColor background) colours, string text)[] result = formatString (format, args);

			foreach (((ConsoleColor foreground, ConsoleColor background) colours, string text) in result)
			{
				stageColours (colours.foreground, colours.background);
				Write<string> (text);
			}
		}

		public void WriteLine (string format, params ConsoleColor[] args)
		{
			((ConsoleColor foreground, ConsoleColor background) colours, string text)[] result = formatString (format, args);

			foreach (((ConsoleColor foreground, ConsoleColor background) colours, string text) in result)
			{
				stageColours (colours.foreground, colours.background);
				Write<string> (text);
			}

			WriteLine ();
		}

		public int Read (ConsoleColor foregroundColour, ConsoleColor backgroundColour)
		{
			stageColours (foregroundColour, backgroundColour);
			return Read ();
		}

		public ConsoleKeyInfo ReadKey (ConsoleColor foregroundColour, ConsoleColor backgroundColour, bool intercept = false)
		{
			stageColours (foregroundColour, backgroundColour);
			return ReadKey (intercept);
		}

		public string ReadLine (ConsoleColor foregroundColour, ConsoleColor backgroundColour)
		{
			stageColours (foregroundColour, backgroundColour);
			return ReadLine ();
		}

		public override void ResetColour ()
		{
			BackgroundColour = defaultBackgroundColour;
			ForegroundColour = defaultForegroundColour;
		}
#pragma warning restore IDE1006 // Naming Styles
	}
}
