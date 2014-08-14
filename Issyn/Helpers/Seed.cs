using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace Issyn2
{
	public class Seed
	{
		/// <summary>
		/// Reads the seed from the seedfile
		/// </summary>
		/// <returns>The seed.</returns>
		public string[] GetSeed(){
			string[] file = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory,"seedfile.txt"));
			return file;
		}
		/// <summary>
		/// Writes the seed into the seedfile
		/// </summary>
		public void WriteSeed(){		
			List<string> seed = new List<string>();	
			foreach (string s in Index.NextRunSeed) {
				if (Index.SiteIndex.Count(l => l.Target == new Uri(s)) == 0)
					seed.Add (s);
			}
			Index.NextRunSeed = seed;
			File.Delete(Path.Combine(Environment.CurrentDirectory,"seedfile.txt"));
			if (Properties.LeaveSite) {
				string[] links = new string[Index.ForeignLinks.Count];
				for (int i = 0; i < links.Count(); i++) {
					links [i] = Index.ForeignLinks [i].ToString ();
				}
				File.WriteAllLines (Path.Combine (Environment.CurrentDirectory, "seedfile.txt"), links);
			}

			File.WriteAllLines (Path.Combine (Environment.CurrentDirectory, "seedfile.txt"), seed.ToArray());			
		}
	}
}

