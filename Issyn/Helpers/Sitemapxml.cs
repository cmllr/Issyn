using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace Issyn2
{
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
	}
}

