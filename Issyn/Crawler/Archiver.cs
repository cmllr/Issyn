using System;
using System.IO;

namespace Issyn2
{
	public class Archiver : IDataLayer
	{
		public Archiver ()
		{
		}

		#region IDataLayer implementation

		public bool Save (Uri url,string content)
		{
			if (url.ToString ().Contains ("#") && url.ToString().EndsWith("#") == false)
				return false;
			url = new Uri(url.ToString().Replace("#",""));
			Console.WriteLine ("[I]: Saving {0}, length {1}",url,content.Length);
			if (content == string.Empty)
				return false;
			string CachePath = Path.Combine(Environment.CurrentDirectory,"Cache");
			string AbsolutePath = Path.Combine (CachePath, url.AbsoluteUri);
			if (Directory.Exists(AbsolutePath)==false && url.ToString().EndsWith("/"))
				Directory.CreateDirectory (AbsolutePath);
			string FilePath = (AbsolutePath.EndsWith ("/")) ? "index.html" : "";
			if (File.Exists (Path.Combine (AbsolutePath, FilePath)))
				Output.Print(string.Format("[E]: Already existing: {0}. Maybe you have to clean the Cache", Path.Combine (AbsolutePath, FilePath)),true);
			File.WriteAllText(Path.Combine(AbsolutePath,FilePath), content);
			Properties.CurrentAlreadyExistingFiles++;
			return true;
		}

		#endregion
	}
}

