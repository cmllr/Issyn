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
			RunParameters.DataAccess = new MongoDB ();//new Issyn2.NDataBaseLayer(){ConnectionString = "./test.db"};
			Properties.MaxCrawlDelay = 10;
			/**************************Read the index**************************/
		
			//var test = new MongoDB ();
			//foreach (Link l in results)
			//	test.StoreLink (l);
			//var links = new MongoDB ().GetLink(new Uri("http://0fury.de/"));



			var results = RunParameters.DataAccess.GetLinks ();		
			//Load the index from the database


		
			if (results != null)
				results.ToList ().ForEach (l => Index.SiteIndex.Add (l));

			/**************************read the seed!**************************/
			string[] seed = new Seed ().GetSeed ();
			Output.PrintBranding ();
			int oldIndexCount = results.Count ();
			if (seed.Length == 0) {
				Output.Print ("[E]: Nothing to index.", true);
				Environment.Exit (129);
			}

			Index.Seed = seed.ToList();			
			Output.Print (string.Format ("Found {0} entries in the database. Adding to index...", results.Count()),false);
			Output.Print (string.Format ("Found {0} entries in the seed file. Adding to crawl list...", seed.Length),false);

			if (System.IsIstanceRunning()) {
				Output.Print ("[E]: Another instance is running! Aborting..", true);
				Environment.Exit (129);
			}

			/**************************start the crawling!**************************/
			System.CreateLockFile ();

			foreach (string s in Index.Seed) {
				new Harvester (new Uri (s), new Uri(s)).StartHarvesting ();
				RunParameters.Robotstxt = string.Empty;
			}		

			/**************************Write the new seed file**************************/
			new Seed ().WriteSeed ();
			Output.Print (string.Format ("Found {0} entries in web. Added to index.", Index.SiteIndex.Count() - oldIndexCount),false);
			Output.Print (string.Format ("Run finshed. Used {0} requests, {1} new entries in seedfile.", RunParameters.RequestCount, new Seed ().GetSeed ().Length), false);
			System.DeleteLockFile ();
		}
	}
}
