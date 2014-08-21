using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
		/// <summary>
		/// Gets the unique links from site.
		/// </summary>
		/// <returns>The unique links from site.</returns>
		/// <param name="content">Content.</param>
		/// <param name="root">Root.</param>
		public static string[] GetUniqueLinksFromSite(string content,Uri root){
			string[] linksFound = new HrefExtract().GetElements (content,root);
			List<string> links = new List<string> ();
			foreach (string s in linksFound) {
				if (!links.Contains (s))
					links.Add (s);
			}
			return links.ToArray ();
		}	
		/// <summary>
		/// Generates an MD5-hashcode of the given string.
		/// </summary>
		/// <returns>The hash of the string</returns>
		/// <param name="input">The string to be hashed.</param>
		public static string Hash( string input)
		{

			// Convert the input string to a byte array and compute the hash.
			byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}
		/// <summary>
		/// Check if a link should be reparsed.
		/// </summary>
		/// <returns><c>true</c> if this instance is link expired the specified l; otherwise, <c>false</c>.</returns>
		/// <param name="l">L.</param>
		public static bool IsLinkExpired(Link l){
			return (l.Expires >= DateTime.Now);
		}
	}
}

