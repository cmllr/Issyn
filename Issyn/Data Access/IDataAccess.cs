using System;

namespace Issyn2
{
	/// <summary>
	/// The interfae which dictates the needed methods for the data layer implementation.
	/// </summary>
	public interface IDataAccess
	{
		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
		string ConnectionString { get; set;}
		/// <summary>
		/// Stores the link.
		/// </summary>
		/// <param name="content">Content.</param>
		void StoreLink(Link content);
		/// <summary>
		/// Updates the link.
		/// </summary>
		/// <param name="content">Content.</param>
		void UpdateLink(Link content);
		/// <summary>
		/// Deletes the link.
		/// </summary>
		/// <param name="content">Content.</param>
		void DeleteLink(Link content);
		/// <summary>
		/// Gets the links.
		/// </summary>
		/// <returns>The links.</returns>
		Link[] GetLinks();
		/// <summary>
		/// Gets the link.
		/// </summary>
		/// <returns>The link.</returns>
		/// <param name="url">URL.</param>
		Link GetLink(Uri url);
		/// <summary>
		/// Determines whether this instance is stored the specified url.
		/// </summary>
		/// <returns><c>true</c> if this instance is stored the specified url; otherwise, <c>false</c>.</returns>
		/// <param name="url">URL.</param>
		bool IsStored(Uri url);
		/// <summary>
		/// Add a link to the index.
		/// </summary>
		/// <param name="keywords">Keywords.</param>
		/// <param name="target">The FQDN of the Hyperlink.</param>
		/// <param name="referrer">Referrer.</param>
		/// <param name="content">The HTML-Sourcecode.</param>
		/// <param name="childs">The hyperlinks to other pages which are contained in the current site.</param>
		/// <param name="images">The images which are linked in.</param>
		void NewSiteToIndex(string[] keywords,Uri target, Uri referrer,string content, string[] childs,string[] images);
		/// <summary>
		/// Updates the last seen.
		/// </summary>
		/// <param name="url">URL.</param>
		void UpdateLastSeen(Uri url);
		/// <summary>
		/// Adds the back link if needed.
		/// </summary>
		/// <param name="site">Site.</param>
		/// <param name="currentUrl">Current URL.</param>
		void AddBackLinkIfNeeded(Uri site,Uri currentUrl);
		/// <summary>
		/// Needses the update.
		/// </summary>
		/// <returns><c>true</c>, if update was needsed, <c>false</c> otherwise.</returns>
		/// <param name="target">Target.</param>
		/// <param name="newHash">New hash.</param>
		bool NeedsUpdate(Uri target,string newHash);
	}
}

