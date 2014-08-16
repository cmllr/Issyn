using System;

namespace Issyn2
{
	/// <summary>
	/// Dictates the method to extract data from the given HTML-content
	/// </summary>
	public interface IExtractModule
	{
		/// <summary>
		/// Get the searched elements in a string-array
		/// </summary>
		/// <returns>The elements.</returns>
		/// <param name="content">The site content.</param>
		/// <param name="root">The site's URL.</param>
		string[] GetElements(string content,Uri root);
	}
}

