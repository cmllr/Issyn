using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
namespace Issyn2
{
	/// <summary>
	/// Extracts meta keywords from the documents
	/// </summary>
	public class KeywordExtract : IExtractModule
	{
		#region IExtractModule implementation

		public string[] GetElements (string content, Uri root)
		{

			string regex = @"<meta\s*name\s*=\s*(""|\')keywords(""|\')\s*content\s*=\s*(""|\')(?<keywords>[^(""|\')]+)(""|\')>";		
			MatchCollection matches = new Regex (regex, RegexOptions.IgnoreCase).Matches (content);
			if (!Regex.IsMatch(content,regex))
				return new string[0];
			string keywords = matches [0].Groups ["keywords"].Value;
			string[] words = keywords.Split (',');
			return words;
		}
		public string[] GetBodyKeywords(string content,Uri url){
			List<string> keywords = new List<string> ();
			string[] values = content.Split (' ');
			foreach (string s in values) {
				if (!s.Contains ("<") && !s.Contains (">") && !IsHTMLTag(s) && !keywords.Contains(s) && !IsKeyWordCommon(s,url)) {
					keywords.Add (s);
				}
			}
			return keywords.ToArray();
		}
		private bool IsKeyWordCommon(string value,Uri url){
			Link linksWithSameUrl = Index.SiteIndex.FirstOrDefault(l => l.Target.Authority == url.Authority);
			if (linksWithSameUrl == null)
				return false;
			else {
				if (linksWithSameUrl.Keywords.Contains (value))
					return true;
				else
					return false;
			}
		}
		private bool IsHTMLTag(string value){
			return Regex.IsMatch(value,@"\w+\s*=\s*(""|')");
		}
		#endregion


	}
}

