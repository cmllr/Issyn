using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
namespace Issyn2
{
	public class Robotstxt
	{	
		public Robotstxt(){
		}
		/// <summary>
		/// Determines whether this instance can be parsed the specified uri.
		/// </summary>
		/// <returns><c>true</c> if this instance can be parsed the specified uri; otherwise, <c>false</c>.</returns>
		/// <param name="uri">URI.</param>
		public bool CanBeParsed(Uri uri){
					
			if (Properties.Robotstxt == String.Empty)
				Properties.Robotstxt = new Downloader ().DownloadSite (new Uri (string.Format ("{0}://{1}/{2}", uri.Scheme, uri.Authority, "robots.txt")));
			//If the file could not be downloaded, shit happens		
			if (Properties.Robotstxt == string.Empty)
				return true;
			else {
				string localPath = uri.LocalPath;
				string[] matches = GetImportantRobotsPart (Properties.Robotstxt);
				bool isAllowed = true;
				foreach(string element in matches){
					if (element == "" || element == string.Empty)
					{
						//Every bot is allowed
						if (this.IsSiteAllowedByMeta (new Downloader ().DownloadSite (uri)))
							isAllowed = true;
						else
							isAllowed = false;
					}
					if (element.Contains("*")){
						//TODO: Wildcard support

					}
					if (element == uri.LocalPath || uri.LocalPath.ToString().StartsWith(element)){
							//The path is not allowed
						isAllowed = false;
						break;
					}
				}
				if (matches.Count(c => c.ToLower ().Trim ().StartsWith ("sitemap")) > 0) {
					//Sitemap
					string regex = @"Sitemap:\s*(?<sitemap>.*)";
					string content = matches.First (c => c.ToLower ().Trim ().StartsWith ("sitemap")).ToString();
					string sitemapUri = Regex.Match (content, regex, RegexOptions.IgnoreCase).Groups["sitemap"].Value;
					//Add the result to the seed!
					List<Uri> seed = new Sitemapxml().Parse(new Downloader().DownloadSite(new Uri(sitemapUri)));
					if (Index.SitemapSeed == null) {
						Output.Print ("[I]: Found a sitemap. Parsing elements...", false);
						Index.SitemapSeed = new List<string> ();
						foreach (Uri s in seed) {
							if (!Index.SitemapSeed.Contains (s.ToString ())) {
								Index.SitemapSeed.Add (s.ToString ());
							}
						}
					} else {
						Output.Print ("[I]: The sitemap was already parsed!", false);
					}
				}
				//if no rule applies, the site can be processed
				return isAllowed;
			}
		}

		/// <summary>
		/// Get the local paths which are listed in the Robots file to get disallowed
		/// </summary>
		/// <returns>The important robots part.</returns>
		/// <param name="robots">Robots.</param>
		private string[] GetImportantRobotsPart(string robots){
			MatchCollection matches = new Regex (@"user-agent:\s+(?<useragent>.*)+", RegexOptions.IgnoreCase).Matches (robots);
			List<String> disallowed = new List<string>();
			for (int i = 0; i < matches.Count; i++) {
				if (matches [i].Value.ToLower ().Contains (Properties.UserAgent.ToLower ()) || matches [i].Value.Contains ("*") ) {				
					int startIndex = robots.IndexOf (matches [i].Value) + matches[i].Value.Length;
					int endIndex = (i < matches.Count -1 ) ? robots.IndexOf (matches [i + 1].Value) :  robots.Length    ; 
					string importantContent = robots.Substring (startIndex, endIndex - startIndex).Trim();
					string disallowedLocalPaths = Regex.Replace(importantContent,@"Disallow:\s+","");
					string[] paths = disallowedLocalPaths.Split ('\n');

					foreach (string s in paths) {
						string Path = s.ToLower ().Trim();
						if (!Path.StartsWith ("crawl-delay"))
							disallowed.Add (s);
						else {
							int delay = GetCrawlDelay (Path);
							Properties.CrawlDelay = (delay != 0) ? delay : Properties.CrawlDelay;
						}
							
					}
						
				}
			}
			return disallowed.ToArray();
		}
		/// <summary>
		/// Determines whether this instance is site allowed by meta the specified sitecontent.
		/// </summary>
		/// <returns><c>true</c> if this instance is site allowed by meta the specified sitecontent; otherwise, <c>false</c>.</returns>
		/// <param name="sitecontent">Sitecontent.</param>
		public bool IsSiteAllowedByMeta(string sitecontent){
			string metaRegex = @"<meta\s?name=""robots""\s?content=""(?<directive>[^""]+)"">";
			MatchCollection matches = new Regex (metaRegex, RegexOptions.IgnoreCase).Matches (sitecontent);
			if (matches.Count == 0)
				return false;
			if (matches [0].Groups ["directive"].Value.ToLower() == "index" ||matches [0].Groups ["directive"].Value.ToLower() == "all" )
				return true;
			return false;
		}
		/// <summary>
		/// Get The Crawl-delay value
		/// </summary>
		/// <returns>The crawl delay or -1.</returns>
		/// <param name="robots">Robots.</param>
		public int GetCrawlDelay(string robots){
			string delayRegex = @"Crawl-delay:[^\d]*(?<crawl_delay>\d+)";
			MatchCollection matches = new Regex (delayRegex, RegexOptions.IgnoreCase).Matches (robots);
			if (matches.Count == 0)
				return 0;
			else
				return int.Parse (matches [0].Groups ["crawl_delay"].Value);
		}
	}
}