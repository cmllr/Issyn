using System;
using System.IO;
using System.Reflection;

namespace Issyn2
{
	public static class Output
	{
		public static void Print(string message,bool wasError){
			ConsoleColor old = Console.ForegroundColor;
			if (wasError)
				Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine (message);
			Console.ForegroundColor = old;
		}
		public static void PrintBranding(){
			string[] content = File.ReadAllLines (Path.Combine (Path.Combine (Environment.CurrentDirectory, "Data"), "BRANDING"));
			foreach(string s in content)
				Console.WriteLine(s);
			Console.WriteLine ("Issyn {0}", Assembly.GetCallingAssembly ().GetName ().Version.ToString ());
			Console.WriteLine (Properties.UserAgent);
		}
	}
}

