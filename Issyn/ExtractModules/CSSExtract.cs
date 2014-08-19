using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Issyn2
{
	/// <summary>
	/// Implementation to extract CSS-directives from the sourcecode
	/// </summary>
	public class CSSExtract : IExtractModule
	{
		#region IExtractModule implementation

		public string[] GetElements (string content,Uri root)
		{
			List<string> css = new List<string> ();
			string regex = @"<link.*href\s?=\s?(""|\')(?<stylesheet>[^(""|\')]*)(""|\').*type=(""|\')text/css(""|\')";
			MatchCollection matches = new Regex (regex).Matches (content);
			for (int i = 0; i < matches.Count; i++) {
				if (matches [i].Groups ["stylesheet"].Value.Trim () != string.Empty) {
					try{
						string href = matches [i].Groups ["stylesheet"].Value;					
						Uri site = new Uri (System.AbsolutizeHref(href,root));
						if (!css.Contains (matches [i].Groups ["stylesheet"].Value) && matches [i].Groups ["stylesheet"].Value != root.ToString ()) {
							if (Properties.LeaveSite == true || site.Authority.ToLower () == root.Authority.ToLower () || site.Authority.ToString().StartsWith("./"))
								css.Add (site.ToString());//matches [i].Groups ["href"].Value.ToLower ());
						}
					}
					catch{
						//The format is shit..
					}
				}
			}
			return css.ToArray ();
		}

		#endregion
	}
}

