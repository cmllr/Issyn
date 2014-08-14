using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Issyn2
{
	public class KeywordExtract : IExtractModule
	{
		#region IExtractModule implementation

		public string[] GetElements (string content, bool addForeign, Uri root)
		{

			string regex = @"<meta\s?name=(""|\')keywords(""|\')\s?content=(""|\')(?<keywords>[^(""|\')]+)(""|\')>";		
			MatchCollection matches = new Regex (regex, RegexOptions.IgnoreCase).Matches (content);
			if (!Regex.IsMatch(content,regex))
				return new string[0];
			string keywords = matches [0].Groups ["keywords"].Value;
			string[] words = keywords.Split (',');
			return words;
		}


		#endregion


	}
}

