using System;
using System.Threading;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using NDatabase;


namespace Issyn2
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			/**************************setup**************************/
			Properties.BlackList = File.ReadAllText (Path.Combine (Path.Combine (Environment.CurrentDirectory, "Data"), "BLACKLIST")).ToLower().Trim().Split(';');
			Properties.DataAccess =  new Issyn2.NDataBaseLayer(){ConnectionString = "./test.db"};
			//Properties.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.67 Safari/537.36";//string.Format ("Mozilla/5.0 Issyn {0}","2.1");
			Properties.MaxCrawlDelay = 10;

			var results = Properties.DataAccess.GetLinks ();		
			//Load the index from the database
			if (results != null)
				results.ToList ().ForEach (l => Index.SiteIndex.Add (l));

			int c = 0;
			results.ToList ().ForEach (x => c += x.Parent.Count ());

			string[] seed = new Seed ().GetSeed ();
			int oldIndexCount = results.Count ();
			if (seed.Length == 0) {
				Output.Print ("[E]: Nothing to index.", true);
				Environment.Exit (129);
			}

			Index.Seed = seed.ToList();	
			Output.PrintBranding ();
			Output.Print (string.Format ("Found {0} entries in the database. Adding to index...", results.Count()),false);

			Output.Print (string.Format ("Found {0} entries in the seed file. Adding to crawl list...", seed.Length),false);


			if (System.IsIstanceRunning()) {
				Output.Print ("[E]: Another instance is running! Aborting..", true);
				Environment.Exit (129);
			}


			/**************************start the crawling!**************************/
			System.CreateLockFile ();

					
			foreach (string s in Index.Seed) {
				new Harvester (new Uri (s), 1, Properties.LeaveSite,new Uri(s)).StartHarvesting ();
			}		

			/**************************Write the new seed file**************************/
			new Seed ().WriteSeed ();
			Output.Print (string.Format ("Found {0} entries in web. Added to index.", Index.SiteIndex.Count() - oldIndexCount),false);
			Output.Print (string.Format ("Run finshed. Used {0} requests, {1} new entries in seedfile.", Properties.RequestCount, new Seed ().GetSeed ().Length), false);
			System.DeleteLockFile ();
		}
	}
}
