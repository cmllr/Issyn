using System;
using System.Text.RegularExpressions;
using System.Linq;
namespace Issyn2
{
	/// <summary>
	/// Extracts meta name="robots"-Informations and provides informations about if the website is allowed to be parsed.
	/// </summary>
	public class MetaExtract : IExtractModule
	{
		#region IExtractModule implementation

		public string[] GetElements (string content, Uri root)
		{
			string metaRegex = @"<meta\s?name=""robots""\s?content=""(?<directive>[^""]+)";
			MatchCollection matches = new Regex (metaRegex, RegexOptions.IgnoreCase).Matches (content);
			if (matches.Count == 0)
				return new string[]{};
			else{
				return matches [0].Groups ["directive"].Value.ToLower ().Split (',');
			}
		}

		#endregion

		/// <summary>
		/// Determines whether this instance is site allowed by meta the specified sitecontent.
		/// </summary>
		/// <returns><c>true</c> if this instance is site allowed by meta the specified sitecontent; otherwise, <c>false</c>.</returns>
		/// <param name="sitecontent">Sitecontent.</param>
		public bool IsSiteAllowedByMeta(string sitecontent,Uri root){

			string[] directives = this.GetElements (sitecontent, root);
			if (directives.Length == 0)
				return true;
			else {
				bool index = (directives.Count (l => l.Trim () == "index") == 1);
				bool noindex = (directives.Count (l => l.Trim () == "noindex") == 1);
				bool all = (directives.Count (l => l.Trim () == "all") == 1);
				if (noindex)
					return false;
				if ((index || all) && !noindex)
					return true;
				else if (!noindex && !all && !index)
					return true;
				else
					return false;
			}
		}
	}
}

