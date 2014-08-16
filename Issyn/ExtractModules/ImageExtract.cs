using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Issyn2
{
	/// <summary>
	/// Extracts images from the document
	/// </summary>
	public class ImageExtract : IExtractModule
	{
		#region IExtractModule implementation

		public string[] GetElements (string content,Uri root)
		{
			//NOOB-Box testen
			List<string> images = new List<string> ();
			string regex = @"<img.*src\s?=\s?(""|\')(?<href>[^(""|\')]*)(""|\')";
			MatchCollection matches = new Regex (regex).Matches (content);
			for (int i = 0; i < matches.Count; i++) {
				if (matches [i].Groups ["href"].Value.Trim ().StartsWith ("#") == false) {
					try{
						string href = matches [i].Groups ["href"].Value;					
						Uri site = new Uri (System.AbsolutizeHref(href,root));
						if (!images.Contains (matches [i].Groups ["href"].Value) && matches [i].Groups ["href"].Value != root.ToString ()) {
							if (Properties.LeaveSite == true || site.Authority.ToLower () == root.Authority.ToLower () || site.Authority.ToString().StartsWith("./"))
								images.Add (site.ToString());//matches [i].Groups ["href"].Value.ToLower ());
						}
					}
					catch (Exception ex){
						//The uri format is shit..
					}
				}
			}
			return images.ToArray ();
		}
		#endregion


	}
}

