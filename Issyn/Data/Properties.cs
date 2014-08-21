using System;
using System.IO;

namespace Issyn2
{
	/// <summary>
	/// The crawl mode determines if the crawler finds links by itself or if it just uses the content of the sitemap. Will be automatically set.
	/// </summary>
	public enum CrawlMode{
		/// <summary>
		/// Sitemap-Mode
		/// </summary>
		SiteMap = 1,
		/// <summary>
		/// The crawling-Mode
		/// </summary>
		Crawl = 2,
	}
	/// <summary>
	/// Properites for the crawler
	/// </summary>
	public static class Properties
	{
		/// <summary>
		/// The user agent to identify with
		/// </summary>
		public static string UserAgent="Mozilla/5.0 (compatible; Issyn2/0)";
		/// <summary>
		/// The crawling delay (min value)
		/// </summary>
		public static int CrawlDelay = 2;
		/// <summary>
		/// The max crawl delay (max value)
		/// </summary>
		public static int MaxCrawlDelay = 5;
		/// <summary>
		/// Determines if the crawler is allowed to leave the site
		/// </summary>
		public static bool LeaveSite = false;
		/// <summary>
		/// The number of maxmimum requests per run
		/// </summary>
		public static int MaxRequestCount = 50;
		/// <summary>
		/// This string contains blacklisted words
		/// </summary>
		public static string[] BlackList;
		/// <summary>
		/// The mode of the crawler. Will be set automatically
		/// </summary>
		public static  CrawlMode Mode = CrawlMode.Crawl;
		/// <summary>
		/// The timespan until a element will expire and needs revalidation
		/// </summary>
		public static TimeSpan ExpiresTimeSpan = new TimeSpan(30,0,0,0);
		/// <summary>
		/// Determines if the childs of a site should be parsed.
		/// </summary>
		public static bool NoChildCrawl = false;
	}
}

