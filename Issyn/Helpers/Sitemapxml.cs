using System;

namespace Issyn2
{
	public class Sitemapxml
	{
		public Sitemapxml ()
		{
		}
		/// <summary>
		/// Parse the specified SiteMap
		/// </summary>
		/// <param name="url">URL.</param>
		public void Parse(Uri url){
			Properties.Mode = CrawlMode.SiteMap;
		}
	}
}

