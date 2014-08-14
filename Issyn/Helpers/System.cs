using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Issyn2
{
	public static class System
	{
		/// <summary>
		/// Determines if is istance running.
		/// </summary>
		/// <returns><c>true</c> if is istance running; otherwise, <c>false</c>.</returns>
		public static bool IsIstanceRunning(){
			if (File.Exists (Path.Combine (Environment.CurrentDirectory, ".lock"))) {
				return true;
			}
			return false;
		}
		/// <summary>
		/// Creates the lock file.
		/// </summary>
		public static void CreateLockFile(){
			File.Create (Path.Combine (Environment.CurrentDirectory, ".lock"));
		}
		/// <summary>
		/// Deletes the lock file.
		/// </summary>
		public static void DeleteLockFile(){
			File.Delete (Path.Combine (Environment.CurrentDirectory, ".lock"));
		}	
		/// <summary>
		/// Absolutizes the href.
		/// </summary>
		/// <returns>The href.</returns>
		/// <param name="old">the Old href.</param>
		/// <param name="root">The current root URL</param>
		public static string AbsolutizeHref(string old,Uri root){
			string replacement = string.Format ("{0}://{1}{2}", root.Scheme, root.Authority, root.PathAndQuery);
			int lastSlash = replacement.LastIndexOf ("/") +1;
			replacement = replacement.Remove(lastSlash, replacement.Length - lastSlash);
			return (old.StartsWith ("./")) ? old.Replace ("./", replacement) : old;
		}
	}
}

