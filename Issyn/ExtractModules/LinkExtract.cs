using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Issyn2
{
	/// <summary>
	/// Extracts hyperlinks from the database
	/// </summary>
	public class LinkExtract : IExtractModule
	{
		#region IExtractModule implementation

		public string[] GetElements (string content,  Uri root)
		{
			List<string> links = new List<string> ();
			string regex = @"<a.*href\s?=\s?(""|\')(?<href>[^(""|\')^#^@]+)(""|\')[^\>]*\>";
			MatchCollection matches = new Regex (regex).Matches (content);
			for (int i = 0; i < matches.Count; i++) {
				if (matches [i].Groups ["href"].Value.Trim ().StartsWith ("#") == false) {
					try{
						string href = matches [i].Groups ["href"].Value;
						//Correct relative links
						if (!href.StartsWith("http") && !href.StartsWith("/") && !href.StartsWith("./")){
							int splitPoint = (root.ToString().Length > root.ToString().LastIndexOf("/") ) ? root.ToString().LastIndexOf("/") : root.ToString().Length;
							string RootToCorrect = root.ToString().Remove(splitPoint);
							href = string.Format("{0}/{1}",RootToCorrect.ToString(),href);
						}
						Uri site = new Uri (System.AbsolutizeHref(href,root));
 						if (!links.Contains (site.ToString()) && site.ToString() != root.ToString ()) {
							if (Properties.LeaveSite == true || site.Authority.ToLower () == root.Authority.ToLower () || site.Authority.ToString().StartsWith("./"))
								links.Add (site.ToString());
							else{
								if (!Index.ForeignLinks.Contains(site))
									Index.ForeignLinks.Add(site);
							}
						}						
					}
					catch (Exception ex){
						//The uri format is shit..
					}
				}
			}
			return links.ToArray ();
		}

		#endregion

	}
}

