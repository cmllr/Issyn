using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Issyn2
{
	public class Link
	{
		public Uri Target;
		public String Text;
		public List<Uri> Parent;
		public DateTime Created;
		public DateTime LastSeen;
		public string[] Keywords;
		public string SiteContent;
		public string[] Images;
	}
}

