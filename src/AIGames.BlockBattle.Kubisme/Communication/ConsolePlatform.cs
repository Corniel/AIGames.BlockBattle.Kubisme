using System;
using System.Collections.Generic;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public class ConsolePlatform : IDisposable
	{/// <summary>Runs the bot.</summary>
		public static void Run(IBot bot)
		{
			using (var platform = new ConsolePlatform())
			{
				platform.DoRun(bot);
			}
		}

		/// <summary>The reader.</summary>
		protected TextReader Reader { get; set; }
		/// <summary>The reader.</summary>
		protected TextWriter Writer { get; set; }
		/// <summary>The reader.</summary>
		protected TextWriter Logger { get; set; }

		/// <summary>Constructs a console platform with Console.In and Console.Out.</summary>
		protected ConsolePlatform() : this(Console.In, Console.Out, Console.Error) { }

		/// <summary>Constructs a console platform.</summary>
		protected ConsolePlatform(TextReader reader, TextWriter writer, TextWriter logger)
		{
			if (reader == null) { throw new ArgumentNullException("reader"); }
			if (writer == null) { throw new ArgumentNullException("writer"); }
			if (logger == null) { throw new ArgumentNullException("logger"); }

			this.Reader = reader;
			this.Writer = writer;
			this.Logger = logger;
		}

		/// <summary>Runs it all.</summary>
		public virtual void DoRun(IBot bot)
		{
			if (bot == null) { throw new ArgumentNullException("bot"); }
			DoRun(bot, Instruction.Read(this.Reader));
		}

		/// <summary>Runs it all.</summary>
		public void DoRun(IBot bot, IEnumerable<IInstruction> instructions)
		{
			if (bot == null) { throw new ArgumentNullException("bot"); }
			if (instructions == null) { throw new ArgumentNullException("instructions"); }

			var settings = new Settings();
			var state = new GameState();

			foreach (var instruction in instructions)
			{
				if (settings.Apply(instruction))
				{
					bot.ApplySettings(settings);
				}
				else if (state.Apply(instruction)) { }
				else if (instruction is RequestMoveInstruction)
				{
					bot.Update(state);
#if !DEBUG
					try
					{
#endif
						var response = bot.GetResponse(((RequestMoveInstruction)instruction).Time);
						Writer.WriteLine(response.Move);
						if (!String.IsNullOrEmpty(response.Log))
						{
							Logger.WriteLine(response.Log);
						}
#if !DEBUG
					}
					catch (Exception x)
					{
						Writer.WriteLine(MoveInstruction.NoMoves);
						Logger.WriteLine(x);
					}
#endif
				}
			}
		}

		#region IDisposable

		/// <summary>Dispose the console platform.</summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Dispose the console platform.</summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!m_IsDisposed)
			{
				if (disposing)
				{
					this.Reader.Dispose();
					this.Writer.Dispose();
				}
				m_IsDisposed = true;
			}
		}

		/// <summary>Destructor</summary>
		~ConsolePlatform() { Dispose(false); }

		private bool m_IsDisposed = false;

		#endregion
	}
}
