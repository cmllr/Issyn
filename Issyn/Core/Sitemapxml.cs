using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace Issyn2
{
	/// <summary>
	/// This class parses the Sitemap.xml-File, if existing.
	/// If the crawler detects a sitemap, it will not be jump to found links anymore.
	/// </summary>
	public class Sitemapxml
	{
		public Sitemapxml ()
		{
			Properties.Mode = CrawlMode.SiteMap;
		}
		/// <summary>
		/// Parse the specified SiteMap
		/// </summary>
		/// <param name="url">URL.</param>
		public List<Uri> Parse(string content){
			List<Uri> urls = new List<Uri> ();
			try{			
				string regex = @"\<loc\>(?<link>[^\<]*)\</loc\>";
				MatchCollection mc = Regex.Matches (content, regex);
				foreach (Match element in mc) {
					if (element.Groups ["link"].Value.EndsWith (".xml")) {
						//Its another sitemap!
						string newSiteMapContent = new Downloader ().DownloadSite (new Uri (element.Groups ["link"].Value));
						urls.AddRange(Parse(newSiteMapContent));
					}
					else{
						urls.Add(new Uri (element.Groups ["link"].Value));
					}
				}
				return urls;
			}
			catch (Exception ex){
				Output.Print (string.Format("[E]: Sitemap parsing failed with {0}.",ex.Message),true );	
				return urls;
			}
		}
	}
}

