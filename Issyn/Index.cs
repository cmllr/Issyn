using System;
using System.Collections.Generic;

namespace Issyn2
{
	/// <summary>
	/// The runtime index of Issyn2
	/// </summary>
	public static class Index
	{
		/// <summary>
		/// The seed from the seedfile.
		/// </summary>
		public static List<String> Seed;
		/// <summary>
		/// The seed which will be stored after execution for the next run
		/// </summary>
		public static List<String> NextRunSeed = new List<string>();
		/// <summary>
		/// The current index of sites.
		/// </summary>
		public static List<Link> SiteIndex = new List<Link>();
		/// <summary>
		/// The links which where found during the run and which are referring to foreign sites.
		/// </summary>
		public static List<Uri> ForeignLinks = new List<Uri>();
		/// <summary>
		/// The seed read out of the sitemap.xml-File
		/// </summary>
		public static List<String> SitemapSeed = null;
	}
}

