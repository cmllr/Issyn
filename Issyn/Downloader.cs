using System;
using System.Net;
using System.IO;
using System.Threading;

namespace Issyn2
{
	public class Downloader
	{
		/// <summary>
		/// Get the contents
		/// </summary>
		/// <returns>The contents.</returns>
		/// <param name="url">URL.</param>
		public string DownloadSite (Uri url)
		{
			try{
				if (!url.IsWellFormedOriginalString ())
					return string.Empty;
				WebClient downloader = new WebClient ();
				downloader.Headers.Add(HttpRequestHeader.UserAgent,Properties.UserAgent);
				string content = downloader.DownloadString (url);
				Properties.RequestCount++;	
				int delay = new Random(DateTime.Now.Millisecond).Next(Properties.CrawlDelay,Properties.MaxCrawlDelay);
				Thread.Sleep (delay*1000);
				Output.Print(string.Format("[I]: Waiting {0} seconds as sheduled",delay),false);
				return content;
			}
			catch(WebException wx){
				Output.Print (string.Format("[E] request failed: {0} on {1}. Waiting 10 seconds",wx.Message,url.ToString()),true );		
				Properties.CurrentFails++;
				Thread.Sleep (10000);
				return string.Empty;
			}
			catch (Exception ex){
				Output.Print (string.Format("[E] {0} on {1}. Waiting 10 seconds",ex.Message,url.ToString()),true );	
				Thread.Sleep (10000);
				return string.Empty;
			}
		}
	}
}

