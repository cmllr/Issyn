using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using System.Security.Cryptography;


namespace Issyn2
{
	public class Harvester
	{
		Uri AttachedToUrl;
		Uri Referrer;
		string content;
		string[] linksFound;
		string[] Images;
		public Harvester (Uri url,Uri referrer)
		{
			this.AttachedToUrl = url;	

			this.Referrer = referrer;
		}
		private bool AreRequestsLeft(){
			if (RunParameters.RequestCount >= Properties.MaxRequestCount) {	
				new Seed().SaveNextRunSeed (this.AttachedToUrl);					
				return false;
			} else {
				return true;
			}
		}

		public void StartHarvesting(){

			//===============================Max request check===============================
			if (!AreRequestsLeft ())
				return;

			//===============================Update if already in index===============================
			//If the site is already indexed -> Only add the page as a referrer
			if (Index.SiteIndex.Count (c => c.Target == this.AttachedToUrl) != 0) {
				//Site already in index
				//Backlink update
				if (Index.SiteIndex.First (c => c.Target == this.AttachedToUrl).Parent.Count (c => c == this.AttachedToUrl) == 0) {				
					Link toUpdate = RunParameters.DataAccess.GetLink (this.AttachedToUrl);	
					if (toUpdate == null)
						return;
					toUpdate.LastSeen = DateTime.Now;
					if (!toUpdate.Parent.Contains (this.Referrer) && toUpdate.Parent.Count (l => l.ToString () == this.Referrer.ToString ()) == 0) {
						Output.Print (string.Format ("[I]: Site {0} is not new, the refferer backlinks will be updated.", this.AttachedToUrl), false);
						toUpdate.Parent.Add (this.Referrer);
						RunParameters.DataAccess.UpdateLink (toUpdate);

					}
					return;
				} else {
					//TODO: FURTHER Update
					GetMetaData ();
				}

			} else {
				//Add the site to the index
				GetMetaData ();
			}
			//If crawling from the sitemap, go to the next
			if (Properties.Mode == CrawlMode.SiteMap){
				//Get the current site index in the sitemap seed
				if (Index.SitemapSeed != null) {
					int i = (Index.SitemapSeed.IndexOf (this.AttachedToUrl.ToString ()) == -1) ? 0 : Index.SitemapSeed.IndexOf (this.AttachedToUrl.ToString ()) + 1;				
					//Continue crawling when there a more elements to crawl
					if (Index.SitemapSeed.Count > i) {
						//if (Index.SiteIndex.Count (l => l.Target == new Uri (Index.SitemapSeed [i])) == 0) {
						new Harvester (new Uri (Index.SitemapSeed [i]), new Uri (Index.SitemapSeed [i])).StartHarvesting ();
					}
				}
			}
		}

		private void GetChildren(string[] links){
			foreach (String s in links) {
				Uri site = new Uri (s);
				//Backlinks 
				RunParameters.DataAccess.AddBackLinkIfNeeded (site,this.AttachedToUrl);

				//Crawl the sites which where found
				if (Properties.LeaveSite == true || site.Authority.ToLower () == AttachedToUrl.Authority.ToLower ()) {
					//Only crawl the child sites if the mode is enabled!
					if (Properties.Mode == CrawlMode.Crawl && new string[]{"html","htm","php","/","aspx","asp","#"}.Count(l => site.ToString().EndsWith(l)) == 1)
						new Harvester (site, this.AttachedToUrl).StartHarvesting ();
				}
			}
		}

		private void GetMetaData(){
			//Parse the child nodes
			bool isAllowed = new Robotstxt().CanBeParsed(this.AttachedToUrl);
			if (!isAllowed) {
				Output.Print (string.Format("[E]: Site {0} was not allowed", this.AttachedToUrl),true);
			} else {
				Output.Print (string.Format("[I]: Moving to {0}. Mode: {1}",this.AttachedToUrl,Properties.Mode.ToString()),false);
				if (Properties.Mode == CrawlMode.SiteMap) {
					int currentSitemapIndex = Index.SitemapSeed.IndexOf (this.AttachedToUrl.ToString ());
					if (currentSitemapIndex == Index.SitemapSeed.Count -1) {
						Output.Print (string.Format("[I]: {0} was the last sitemap entry. Cleaning sitemap for next one.",this.AttachedToUrl),false);
						Index.SitemapSeed = null;
						RunParameters.WasSiteMapParsed = false;
					}
				}
				this.content = new Downloader ().DownloadSite (this.AttachedToUrl);	
				if (!new MetaExtract ().IsSiteAllowedByMeta (this.content,this.AttachedToUrl)) {
					Output.Print (string.Format("[E]: Site {0} was not allowed. Denied by meta tag", this.AttachedToUrl),true);
					return;
				}
				if (content == string.Empty)
					return;
				//harvesting data!
				this.linksFound = System.GetUniqueLinksFromSite (content,this.AttachedToUrl);
				this.Images = new ImageExtract ().GetElements (content,  this.AttachedToUrl);
				string[] keywords = new KeywordExtract ().GetElements (content, this.AttachedToUrl);
				//Store the link
				bool isExisting = RunParameters.DataAccess.IsStored (this.AttachedToUrl);
				if (!isExisting) {
					Output.Print (string.Format ("[I]: Site {0} is new, will be added to DataBase.", this.AttachedToUrl), false);
					RunParameters.DataAccess.NewSiteToIndex(keywords,this.AttachedToUrl,this.Referrer,this.content,this.linksFound,this.Images);
					this.GetChildren (linksFound);
				}else {
					//Update
					string hash = System.Hash (content);
					if ( RunParameters.DataAccess.NeedsUpdate (this.AttachedToUrl, hash)) {	
						Output.Print (string.Format ("[I]: Site {0} is not new, but something was changed.", this.AttachedToUrl), false);
						RunParameters.DataAccess.UpdateLastSeen (this.AttachedToUrl);
						//TODO: Update the site
						Link s = RunParameters.DataAccess.GetLink (this.AttachedToUrl);
						Link updated = s;
						updated.Checksum = hash;
						updated.SiteContent = content;
						updated.Childs = linksFound;
						RunParameters.DataAccess.UpdateLink (updated);
						if (s != null) {
							foreach (Uri backlink in s.Parent) {
								if (backlink != this.AttachedToUrl && Properties.Mode == CrawlMode.Crawl)
									new Harvester (backlink,this.Referrer).StartHarvesting ();
							}
						}

					} else {
						Output.Print (string.Format ("[I]: Site {0} is not new and no update is needed", this.AttachedToUrl), false);
					}
				}
			}
		}
	}
}
