using System;
using System.IO;
using System.Reflection;

namespace Issyn2
{
	/// <summary>
	/// The class for console output.
	/// </summary>
	public static class Output
	{
		/// <summary>
		/// Prints out the given message
		/// </summary>
		/// <param name="message">The Message.</param>
		/// <param name="wasError">If set to <c>true</c> was error.</param>
		public static void Print(string message,bool wasError){
			ConsoleColor old = Console.ForegroundColor;
			if (wasError)
				Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine (message);
			Console.ForegroundColor = old;
		}
		/// <summary>
		/// Prints the branding.
		/// </summary>
		public static void PrintBranding(){
			string[] content = File.ReadAllLines (Path.Combine (Path.Combine (Environment.CurrentDirectory, "Data"), "BRANDING"));
			foreach(string s in content)
				Console.WriteLine(s);
			Console.WriteLine ("Issyn {0}", Assembly.GetCallingAssembly ().GetName ().Version.ToString ());
			Console.WriteLine (Properties.UserAgent);
		}
	}
}

