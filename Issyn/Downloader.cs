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
				RunParameters.RequestCount++;	
				int delay = new Random(DateTime.Now.Millisecond).Next(Properties.CrawlDelay,Properties.MaxCrawlDelay);
				Thread.Sleep (delay*1000);
				Output.Print(string.Format("[I]: Download of {0} finished. Waiting {1} seconds as sheduled. Used {2} request until now.",url, delay,RunParameters.RequestCount),false);
				return content;
			}
			catch(WebException wx){
				try{
					HttpStatusCode result =  ((HttpWebResponse)wx.Response).StatusCode;
					if (new Politeness (RunParameters.AcceptedErrorStatus).IsOK (result)) {
						Output.Print (string.Format ("[E]: Request failed with {0} on {1}, but status code can be handled.", result.ToString(), url.ToString ()), true);		
					} else {
						Output.Print (string.Format ("[E]: request failed with {0} on {1}. Waiting 10 seconds", result.ToString(), url.ToString ()), true);		
						RunParameters.CurrentFails++;
						Thread.Sleep (10000);
					}
					return string.Empty;
				}
				catch(Exception inner){
					Output.Print (string.Format ("[E]: request failed with {0} on {1}. Waiting 10 seconds", inner.Message, url.ToString ()), true);		
					RunParameters.CurrentFails++;
					Thread.Sleep (10000);
					return string.Empty;
				}
			}
			catch (Exception ex){
				Output.Print (string.Format("[E]: {0} on {1}. Waiting 10 seconds",ex.Message,url.ToString()),true );	
				Thread.Sleep (10000);
				return string.Empty;
			}
		}
	}
}

