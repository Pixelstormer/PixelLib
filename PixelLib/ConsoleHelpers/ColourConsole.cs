using System;
using System.Collections.Generic;
using static System.FormattableString;

namespace PixelLib.ConsoleHelpers
{
	/// <summary>
	/// Allows for far finer control of <see cref="BackgroundColour"/> and <see cref="ForegroundColour"/> settings.
	/// </summary>
	public sealed class ColourConsole : CustomConsole
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
			: this (DEFAULT_FOREGROUNDCOLOUR, DEFAULT_BACKGROUNDCOLOUR) { }
		
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

			preWriteEvent += handlePreWriteOperation;
			preWriteLineEvent += handlePreWriteOperation;

			postWriteEvent += handlePostWriteOperation;
			postWriteLineEvent += handlePostWriteOperation;
		}
		
#pragma warning disable IDE0022 // Use block body for methods
		private void handlePreWriteOperation (object sender, EventArgs e) => stageColours (ForegroundColour, BackgroundColour);
		private void handlePostWriteOperation (object sender, EventArgs e) => unstageColours ();
#pragma warning restore IDE0022 // Use block body for methods

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

		private ConsoleText [] formatString (string toFormat, params ConsoleColor [] args)
		{
			// If format doesn't specify a ConsoleColour to start with, default to the current colours.
			if (!toFormat.StartsWith (STRINGFORMAT_STARTBLOCK.ToString (), StringComparison.Ordinal))
				toFormat = Invariant ($"{{{ForegroundColour.ToString ()}:{BackgroundColour.ToString ()}}}") + toFormat;
			
			// Make an estimate of how many blocks there are going to be.
			List<string> stringBlocks = new List<string> (Math.Max (args.Length, 2));

			// Keep track of the indexes of escape chars in this block to remove them later.
			List<int> escapeIndexes = new List<int> ();

			// Index of the start of this block, and escape char state.
			int previousIndex = 0;
			bool escapeNext = false;

			// Iterate over the string, collecting substrings based on the indexes of (unescaped) block starters.
			for (int i = 1; i < toFormat.Length; i++)
			{
				char currentChar = toFormat [i];

				if (escapeNext)
					escapeNext = false;
				else
				{
					switch (currentChar)
					{
						case STRINGFORMAT_ESCAPE:
							escapeNext = true;
							escapeIndexes.Add (i - previousIndex);
							break;

						case STRINGFORMAT_STARTBLOCK:
							string block = toFormat.Substring (previousIndex, i - previousIndex);

							// Remove all of the escape chars in this block, and clear the list for the next block.
							foreach (int index in escapeIndexes)
								block = block.Remove (index, 1);
							escapeIndexes.Clear ();

							stringBlocks.Add (block);
							previousIndex = i;
							break;
					}
				}
			}

			// Starting a new block is the trigger for adding the previous block,
			// so manually adding the last block is needed,
			// as it won't be followed by another block starter.
			stringBlocks.Add (toFormat.Substring (previousIndex));

			// Iterate over the collected blocks and parse them into ConsoleText structs.
			ConsoleText [] textBlocks = new ConsoleText [stringBlocks.Count];
			for (int i = 0; i < textBlocks.Length; ++i)
				textBlocks [i] = parseBlock (stringBlocks [i], args);

			return textBlocks;
		}

		/* OLD FORMATSTRING METHOD.
		List<string> blocks = new List<string> (args.Length);
		List<(ConsoleColor foreground, ConsoleColor background)> blockColours = new List<(ConsoleColor foreground, ConsoleColor background)> (args.Length);

		bool inColourBlock = false;
		bool escapeNextChar = false;
		bool foundSeparator = false;

		string currentBlock = "";

		(ConsoleColor foreground, ConsoleColor background) currentColours = (ForegroundColour, BackgroundColour);

		for (int i = 0; i < toFormat.Length; i++)
		{
			char currentChar = toFormat[i];

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
							throw new FormatException (Invariant($"{nameof (formatString)} received bad format string: '{toFormat}'. (Got {nameof (STRINGFORMAT_ESCAPE)} char ('{STRINGFORMAT_ESCAPE}') while in a block at index: {i}."));
						escapeNextChar = true;
						break;
							
					case STRINGFORMAT_STARTBLOCK:
						if (inColourBlock)
							throw new FormatException (Invariant ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Got {nameof (STRINGFORMAT_STARTBLOCK)} char ('{STRINGFORMAT_STARTBLOCK}') while already in a block at index: {i}.)"));
						inColourBlock = true;
						if (i != 0)
							blockStrings.Add (currentBlock);
						currentBlock = "";
						break;

					case STRINGFORMAT_COLOURSEP:
						if (foundSeparator)
							throw new FormatException (Invariant ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Got additional {nameof (STRINGFORMAT_COLOURSEP)} char before reaching end of block at index {i}.)"));

						if (inColourBlock)
						{
							if (string.IsNullOrWhiteSpace (currentBlock))
								currentColours.foreground = ForegroundColour;
							else if (int.TryParse (currentBlock, out int paramsIndex))
							{
								try
								{ currentColours.foreground = args[paramsIndex]; }
								catch (IndexOutOfRangeException e)
								{ throw new FormatException (Invariant ($"{nameof (formatString)} recieved bad format string: '{toFormat}'. (Specified {nameof (args)} index '{paramsIndex}' is out of range at index {i}.)"), e); }
							}
							else if (Enum.TryParse (currentBlock, true, out ConsoleColor colour))
								currentColours.foreground = colour;
							else
								throw new FormatException (Invariant ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Got invalid Colour Block: '{currentBlock}' at index: {i}.)"));

							currentBlock = "";
							foundSeparator = true;
						}
						else
							currentBlock += currentChar;
						break;

					case STRINGFORMAT_ENDBLOCK:
						if (!inColourBlock)
							throw new FormatException (Invariant ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Got {nameof (STRINGFORMAT_ENDBLOCK)} char ('{STRINGFORMAT_ENDBLOCK}') while not in a block at index: {i}."));

						if (!foundSeparator)
							throw new FormatException (Invariant ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Got {nameof (STRINGFORMAT_ENDBLOCK)} char ('{STRINGFORMAT_ENDBLOCK}') before finding a {nameof (STRINGFORMAT_COLOURSEP)} char ('{STRINGFORMAT_COLOURSEP}') at index {i}.)"));

						if (string.IsNullOrWhiteSpace (currentBlock))
							currentColours.background = BackgroundColour;
						else if (int.TryParse (currentBlock, out int paramsIndex))
						{
							try
							{ currentColours.background = args[paramsIndex]; }
							catch (IndexOutOfRangeException e)
							{ throw new FormatException (Invariant ($"{nameof (formatString)} recieved bad format string: '{toFormat}'. (Specified {nameof (args)} index '{paramsIndex}' is out of range at index {i}.)"), e); }
						}
						else if (Enum.TryParse (currentBlock, true, out ConsoleColor colour))
							currentColours.background = colour;
						else
							throw new FormatException (Invariant ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Got invalid Colour Block: '{currentBlock}' at index: {i}.)"));

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
			throw new FormatException ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Reached end of string while still in block.)");

		if (string.IsNullOrEmpty (currentBlock))
			throw new FormatException ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Block at end of string had no following chars.)");

		blockStrings.Add (currentBlock);

		if (blockStrings.Count != blockColours.Count)
			throw new FormatException (Invariant ($"{nameof (formatString)} received bad format string: '{toFormat}'. (Got unequal number of Colour Blocks ({blockColours.Count}) and Text Blocks ({blockStrings.Count}).)"));

		((ConsoleColor foreground, ConsoleColor background) colours, string text)[] result = new ((ConsoleColor foreground, ConsoleColor background), string)[blockStrings.Count];

		for (int i = 0; i < blockStrings.Count; i++)
		{
			result[i].colours = blockColours[i];
			result[i].text = blockStrings[i];
		}

		return result;
		*/

		private ConsoleText parseBlock (string toParse, params ConsoleColor [] args)
		{
			if (toParse == null)
				throw new ArgumentNullException (nameof (toParse), $"Cannot create a {nameof (ConsoleText)} from a null string.");

			if (!toParse.StartsWith (STRINGFORMAT_STARTBLOCK.ToString (), StringComparison.Ordinal))
				throw new FormatException ($"Invalid format string: '{toParse}'. String does not start with a valid Block.");

			int blockEndIndex = toParse.IndexOf (STRINGFORMAT_ENDBLOCK);
			int blockSepIndex = toParse.IndexOf (STRINGFORMAT_COLOURSEP);

			if (blockEndIndex == -1)
				throw new FormatException ($"Invalid format string: '{toParse}'. String does not start with a valid block.");

			if (blockSepIndex == -1 || blockSepIndex >= blockEndIndex)
				throw new FormatException ($"Invalid format string: '{toParse}'. String does not start with a valid block.");

			string foregroundString = toParse.Substring (1, blockSepIndex - 1);
			string backgroundString = toParse.Substring (blockSepIndex + 1, blockEndIndex - blockSepIndex - 1);
			string remainingText = toParse.Substring (blockEndIndex + 1);

			ConsoleColor foregroundColour = colourFromString (foregroundString, true, args);
			ConsoleColor backgroundColour = colourFromString (backgroundString, false, args);

			return new ConsoleText (foregroundColour, backgroundColour, remainingText);
		}

		private ConsoleColor colourFromString (string from, bool isForeground, params ConsoleColor [] args)
		{
#pragma warning disable IDE0046 // Convert to conditional expression - Nested conditional expressions are ugly.
			if (string.IsNullOrWhiteSpace (from))
				return isForeground ? ForegroundColour : BackgroundColour;
#pragma warning restore IDE0046 // Convert to conditional expression

			return colourFromString (from, args);
		}

#pragma warning disable IDE1006 // Naming Styles - Method overloads imitating the naming styles of System.Console.
		public void Write (string format, params ConsoleColor [] args)
		{
			foreach (ConsoleText textBlock in formatString (format, args))
			{
				stageColours (textBlock.foregroundColour, textBlock.backgroundColour);
				Write<string> (textBlock.text);
			}
		}

		public void WriteLine (string format, params ConsoleColor [] args)
		{
			foreach (ConsoleText textBlock in formatString (format, args))
			{
				stageColours (textBlock.foregroundColour, textBlock.backgroundColour);
				Write<string> (textBlock.text);
			}

			WriteLine ();
		}

		public int Read (ConsoleColor foregroundColour, ConsoleColor backgroundColour)
		{
			stageColours (foregroundColour, backgroundColour);
			return Read ();
		}

		public ConsoleKeyInfo ReadKey (ConsoleColor foregroundColour, ConsoleColor backgroundColour)
		{
			return ReadKey (foregroundColour, backgroundColour, false);
		}

		public ConsoleKeyInfo ReadKey (ConsoleColor foregroundColour, ConsoleColor backgroundColour, bool intercept)
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

		// Rule CA1305 triggers on the string interpolation for the FormatException within the catch block, but not on any of the other similar string interpolations in the solution, for some reason.
		// From: https://docs.microsoft.com/en-gb/visualstudio/code-quality/ca1305?view=vs-2017#rule-description "If the value will be displayed to the user, use the current culture."
		// As this project is a class library, the 'user' is a developer using the library in their own project(s). If they misuse the library, causing the exception to be thrown,
		// the exception message (The guilty string interpolation) will be displayed to the user (The developer). So this is fine, as 'raw' string interpolation defaults to the Current Culture.
		[System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
		private static ConsoleColor colourFromString (string from, params ConsoleColor [] args)
		{
			if (string.IsNullOrWhiteSpace (from))
				throw new ArgumentException ($"Cannot create a {nameof (ConsoleColor)} from the string '{from}': String cannot be null, empty or whitespace.", nameof (from));
			else if (int.TryParse (from, out int paramsIndex))
			{
				try
				{ return args [paramsIndex]; }
				catch (IndexOutOfRangeException e)
				{ throw new FormatException ($"The index {paramsIndex} specified by the string '{from}' was not a valid index for the params array.", e); }
			}
			else if (Enum.TryParse (from, true, out ConsoleColor colour))
				return colour;
			else
				throw new FormatException ($"The {nameof (ConsoleColor)} '{from}' does not exist.");
		}

		private struct ConsoleText : IEquatable<ConsoleText>
		{
			public readonly ConsoleColor foregroundColour;
			public readonly ConsoleColor backgroundColour;
			public readonly string text;

			public ConsoleText (ConsoleColor foregroundColour, ConsoleColor backgroundColour, string text)
			{
				this.foregroundColour = foregroundColour;
				this.backgroundColour = backgroundColour;
				this.text = text;
			}

			public bool Equals (ConsoleText other)
			{
				return foregroundColour == other.foregroundColour
					&& backgroundColour == other.backgroundColour
					&& string.Equals (text, other.text, StringComparison.Ordinal);
			}
		}
	}
}
