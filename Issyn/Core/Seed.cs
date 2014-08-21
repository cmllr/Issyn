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
			return file.Where(l => l != string.Empty).ToArray();
		}
		/// <summary>
		/// Writes the seed into the seedfile
		/// </summary>
		public void WriteSeed(){		
			List<string> seed = new List<string>();	
			seed = Index.NextRunSeed;
			seed = this.SeedShuffle (GetSeed ().ToList ().First());
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
				//add sitemap seed
				if (Index.SitemapSeed != null) {
					//there was a sitemap used...
					Index.NextRunSeed.Add (currentUrl.AbsoluteUri);
				}
			}
		}
		public List<string> SeedShuffle(string current){
			List<string> newSeed = new List<string> ();
			newSeed = Index.NextRunSeed;
			Uri currentUri = new Uri (current);
			string toFind = currentUri.Authority;
			List<string> NotInCurrentSite = newSeed.Where (l => !l.Contains (toFind)).ToList();
			List<string> InCurrent =  newSeed.Where (l => l.Contains (toFind)).ToList();
			newSeed.Clear ();
			newSeed.AddRange (NotInCurrentSite);
			newSeed.AddRange (InCurrent);
			return newSeed;
		}
	}
}

