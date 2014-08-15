using System;
using System.IO;

namespace Issyn2
{
	public enum CrawlMode{
		SiteMap = 1,
		Crawl = 2,
	}
	public static class Properties
	{
		public static string UserAgent="Mozilla/5.0 (compatible; Issyn2/0)";
		public static int WorkerProcesses = 3;
		public static int CrawlDelay = 2;
		public static int MaxCrawlDelay = 5;
		public static int RequestCount = 0;
		public static string Robotstxt = string.Empty;
		public static int MaxAlreadyExistingFiles = 300;
		public static int AlreadyIndexed = 0;
		public static int InIndex = 0;
		public static int MaxFails = 30;
		public static int CurrentFails = 0;
		public static IDataAccess DataAccess ;
		public static bool LeaveSite = false;
		public static int MaxRequestCount = 5;
		public static string[] BlackList;
		public static  CrawlMode Mode = CrawlMode.Crawl;
		public static bool AdditionalContentRequests = true;
	}
}

