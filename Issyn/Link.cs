using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Issyn2
{
	/// <summary>
	/// The Link contains data about a single website
	/// </summary>
	public class Link
	{
		/// <summary>
		/// The target href of the website
		/// </summary>
		public Uri Target;
		/// <summary>
		/// The link text
		/// </summary>
		public String Text;
		/// <summary>
		/// A collection of links which are reffering to the link
		/// </summary>
		public List<Uri> Parent;
		/// <summary>
		/// The datetime when the site was added to the index
		/// </summary>
		public DateTime Created;
		/// <summary>
		/// The datetime when the site was last seen on the web
		/// </summary>
		public DateTime LastSeen;
		/// <summary>
		/// Keywords, taken out of the Meta-Section of the HTML-Head.
		/// </summary>
		public string[] Keywords;
		/// <summary>
		/// The HTML content of the site.
		/// </summary>
		public string SiteContent;
		/// <summary>
		/// The images of the page (only the links)
		/// </summary>
		public string[] Images;
		/// <summary>
		/// A collection of hyperlinks which where found in the site
		/// </summary>
		public string[] Childs;
		/// <summary>
		/// The checksum of the page to identify changes.
		/// </summary>
		public string Checksum;
	}
}

