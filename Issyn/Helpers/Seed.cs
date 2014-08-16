using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace Issyn2
{
	/// <summary>
	/// This class handles the seed which is processed at every run.
	/// </summary>
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
		/// <summary>
		/// Saves the next run seed.
		/// </summary>
		/// <param name="currentUrl">Current URL.</param>
		public void SaveNextRunSeed(Uri currentUrl){
			if (!Index.NextRunSeed.Contains (currentUrl.ToString ())) {
				//ADd only the sites to the next run seed, which are not alrady in the index
				if (Index.SiteIndex.Count (l => l.Target == currentUrl) == 0)
					Index.NextRunSeed.Add (currentUrl.ToString ());
				//add sitemap seed
				if (Index.SitemapSeed != null) {
					//there was a sitemap used...
					Index.NextRunSeed.Add (string.Format ("{0}://{1}", currentUrl.Scheme,currentUrl.Authority));
				}
			}
		}
	}
}

