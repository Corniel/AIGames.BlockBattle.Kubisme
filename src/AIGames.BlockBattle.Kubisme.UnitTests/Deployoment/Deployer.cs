using Microsoft.CSharp;
using NUnit.Framework;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Deployment
{
	[TestFixture]
	public class DeployerTest
	{
		[Test]
		public void Deploy_Cubism()
		{
			var collectDir = new DirectoryInfo(@"..\..\..\AIGames.BlockBattle.Kubisme");
			var full = collectDir.FullName;
			Deployer.Run(collectDir, "Kubisme", "0001", false);
		}
	}

	/// <summary>The bot deployment program.</summary>
	public class Deployer
	{
		/// <summary>The main entry point of the application.</summary>
		public static void Main(string[] args)
		{
			if (args == null || args.Length < 2)
			{
				WriteUsage();
			}

			try
			{
				var dir = new DirectoryInfo(args[0]);
				var botname = args[1];
				var version = args.Length < 3 ? string.Empty : args[2];
				var debug = args.Length < 4 ? false : (args[3] ?? string.Empty).ToUpper() == "DEBUG";
				Run(dir, botname, version, debug);
			}
			catch (Exception x)
			{
				Console.WriteLine(x);
			}
		}

		/// <summary>Runs te program.</summary>
		public static void Run(DirectoryInfo source, string botname, string version, bool debug)
		{
			var bot = GetVersionedBotName(botname, version);

			var target = new DirectoryInfo(Path.Combine(GetBotsDir().FullName, "bin", bot, "collect"));
			if (target.Exists)
			{
				target.Delete(true);
			}
			target.Create();

			CopyCSharpFiles(source, target, debug);

			if (Compile(target, bot, debug))
			{
				Compress(target, bot);
				target.Delete(true);
			}
		}

		/// <summary>Copies the C# files from the source to the target directory.</summary>
		public static void CopyCSharpFiles(DirectoryInfo source, DirectoryInfo target, bool debug = false)
		{
			var botfiles = CollectCSharpFiles(source, debug).ToList();
			
			foreach (var file in botfiles)
			{
				file.CopyTo(Path.Combine(target.FullName, file.Name));
			}
			
		}

		/// <summary>Compiles the bot.</summary>
		/// <returns>
		/// False if ther where compile errors, otherwise true.
		/// </returns>
		public static bool Compile(DirectoryInfo source, string name, bool debug = false)
		{
			using (var provider = new CSharpCodeProvider())
			{
				var options = new CompilerParameters();
				options.GenerateExecutable = true;

				if (debug)
				{
					options.IncludeDebugInformation = true;
				}

				var compileAssemblies = debug ? CompileAssembliesDebug : CompileAssembliesRelease;

				foreach (var assembly in compileAssemblies)
				{
					options.ReferencedAssemblies.Add(assembly.Location);
				}

				options.OutputAssembly = Path.Combine(source.Parent.FullName, name + ".exe");

				var csFiles = source.GetFiles("*.cs").Select(f => f.FullName).ToArray();

				var exe = provider.CompileAssemblyFromFile(options, csFiles);

				if (exe.Errors.HasErrors)
				{
					foreach (var error in exe.Errors.OfType<CompilerError>().Where(e => !e.IsWarning))
					{
						Console.WriteLine(error);
					}
					return false;
				}
				return true;
			}
		}

		private static readonly Assembly[] CompileAssembliesDebug = new Assembly[]
		{
			typeof(System.Int32).Assembly,
			typeof(System.Linq.Enumerable).Assembly,
			typeof(System.Text.RegularExpressions.Regex).Assembly,
		};
		private static readonly Assembly[] CompileAssembliesRelease = new Assembly[]
		{
			typeof(System.Int32).Assembly,
			typeof(System.Linq.Enumerable).Assembly,
			typeof(System.Text.RegularExpressions.Regex).Assembly,
		};

		/// <summary>Zips the C# files to the zip directory.</summary>
		public static void Compress(DirectoryInfo source, string bot)
		{
			var destination = new FileInfo(Path.Combine(GetBotsDir().FullName, "zips", bot + ".zip"));

			if (destination.Exists)
			{
				destination.Delete();
			}
			ZipFile.CreateFromDirectory(source.FullName, destination.FullName);
		}

		/// <summary>Collect the C# files from a given location.</summary>
		/// <param name="dir">
		/// the directory to collect files from.
		/// </param>
		/// <param name="debug">
		/// If true exclude *.Release.cs otherwise *.Debug.cs.
		/// </param>
		/// <returns></returns>
		public static IEnumerable<FileInfo> CollectCSharpFiles(DirectoryInfo dir, bool debug = false)
		{
			foreach (var child in dir.GetDirectories().Where(d => !ExcludeDirs.Contains(d.Name)))
			{
				foreach (var file in CollectCSharpFiles(child, debug))
				{
					yield return file;
				}
			}
			foreach (var file in dir.GetFiles("*.cs"))
			{

				if (debug && !file.Name.EndsWith(".Release.cs") && file.Name != "AssemblyInfo.cs")
				{
					yield return file;
				}
				else if (!file.Name.EndsWith(".Debug.cs") && file.Name != "AssemblyInfo.cs")
				{
					yield return file;
				}
			}
		}
		private static readonly string[] ExcludeDirs = new string[] { "bin", "obj" };

		/// <summary>Gets the versioned bot name.</summary>
		/// <param name="name">
		/// The name
		/// </param>
		/// <param name="version"></param>
		/// <returns></returns>
		public static string GetVersionedBotName(string name, string version)
		{
			return string.IsNullOrEmpty(version) ? name : name + '.' + version;
		}
	
		/// <summary>Gets the configured bots directory.</summary>
		/// <remarks>
		/// The bots directory is the directory containing the bots for Arena.
		/// </remarks>
		public static DirectoryInfo GetBotsDir()
		{
			var dir = ConfigurationManager.AppSettings["Bots.Dir"];
			return new DirectoryInfo(dir);
		}

		private static void WriteUsage()
		{
			Console.Write("usage: source botname [version] [DEBUG]");
		}
	}
}
