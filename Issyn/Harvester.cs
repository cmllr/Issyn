using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
namespace Issyn2
{
	public class Harvester
	{
		int WorkerProcessNumber;
		Uri AttachedToUrl;
		Uri Referrer;
		string content;
		string[] linksFound;
		bool leaveSite;
		string[] Images;
		public Harvester (Uri url,int number,bool leave,Uri referrer)
		{
			this.AttachedToUrl = url;
			this.WorkerProcessNumber = number;
			this.leaveSite = leave;
			this.Referrer = referrer;
		}
		public void StartHarvesting(){

			//===============================Max request check===============================

			if (Properties.RequestCount >= Properties.MaxRequestCount) {	
				if (!Index.NextRunSeed.Contains (this.AttachedToUrl.ToString ())) {
					//ADd only the sites to the next run seed, which are not alrady in the index
					if (Index.SiteIndex.Count(l => l.Target == this.AttachedToUrl) == 0)
						Index.NextRunSeed.Add (this.AttachedToUrl.ToString ());
					//add sitemap seed
					if (Index.SitemapSeed != null) {
						//there was a sitemap used...
						Index.NextRunSeed.Add(string.Format("{0}://{1}",this.AttachedToUrl.Scheme,this.AttachedToUrl.Authority));
					}
				}					
				return;
			}
			//===============================Update if already in index===============================
			//If the site is already indexed -> Only add the page as a referrer
			if (Index.SiteIndex.Count (c => c.Target == this.AttachedToUrl) != 0) {
				if (Index.SiteIndex.First (c => c.Target == this.AttachedToUrl).Parent.Count (c => c == this.AttachedToUrl) == 0) {				
					Link toUpdate = Properties.DataAccess.GetLink (this.AttachedToUrl);	
					if (toUpdate == null)
						return;
					toUpdate.LastSeen = DateTime.Now;
					if (!toUpdate.Parent.Contains (this.Referrer) && toUpdate.Parent.Count (l => l.ToString () == this.Referrer.ToString ()) == 0) {
						Output.Print (string.Format ("[I]: Site {0} is not new, the refferer backlinks will be updated.", this.AttachedToUrl), false);
						toUpdate.Parent.Add (this.Referrer);
						Properties.DataAccess.UpdateLink (toUpdate);
					} else {
						//TODO: Update run!
					}
					return;
				}

			} else {
				//Add the site to the index
				ProcessSite ();
			}
			//If crawling from the sitemap, go to the next
			if (Properties.Mode == CrawlMode.SiteMap){
				//Get the current site index in the sitemap seed
				int i = (Index.SitemapSeed.IndexOf (this.AttachedToUrl.ToString()) == -1 )? 0 : Index.SitemapSeed.IndexOf (this.AttachedToUrl.ToString()) +1;				
				//Continue crawling when there a more elements to crawl
				if (Index.SitemapSeed.Count > i) {
					//if (Index.SiteIndex.Count (l => l.Target == new Uri (Index.SitemapSeed [i])) == 0) {
					Output.Print (string.Format ("[I]: Going to next page (#{0},{1}), from sitemap.", i, Index.SitemapSeed [i].ToString ()), false);
					new Harvester (new Uri (Index.SitemapSeed [i]), this.WorkerProcessNumber, this.leaveSite, new Uri (Index.SitemapSeed [i])).StartHarvesting ();
				}
			}
		}
		private void NewSiteToIndex(string[] keywords){
			Properties.DataAccess.StoreLink (new Link () {
				Target = this.AttachedToUrl,
				Parent = new List<Uri> (){ this.Referrer },
				Text = string.Empty,
				Created = DateTime.Now,
				LastSeen = DateTime.Now,
				Keywords = keywords,
				SiteContent = (Properties.AdditionalContentRequests) ? new Downloader().DownloadSite(this.AttachedToUrl).ToString() : null,
			});
		}
		private void ChildLinkProcessing(string[] links){
			foreach (String s in links) {
				Uri site = new Uri (s);
				//Backlinks 
				Link mayBeBacklink = Index.SiteIndex.FirstOrDefault (t => t.Target == site);

				if (mayBeBacklink != null) {
					if (!mayBeBacklink.Parent.Contains (this.AttachedToUrl) && mayBeBacklink.Parent.Count(l => l.ToString() == this.AttachedToUrl.ToString()) == 0) {
						Output.Print(String.Format("[I]: Backlink found {0}, reffering to {1}.",mayBeBacklink.Target.ToString(),this.AttachedToUrl.ToString()),false);
						mayBeBacklink.Parent.Add (this.AttachedToUrl);
						new NDataBaseLayer ().UpdateLink (mayBeBacklink);
					}
				}

				//Crawl the sites which where found
				if (leaveSite == true || site.Authority.ToLower () == AttachedToUrl.Authority.ToLower ()) {
					//Only crawl the child sites if the mode is enabled!
					if (Properties.Mode == CrawlMode.Crawl)
						new Harvester (site, this.WorkerProcessNumber, leaveSite, this.AttachedToUrl).StartHarvesting ();
				}
			}
		}
		private void UpdateLastSeen(Uri url){
			Link toUpdate = Properties.DataAccess.GetLink (this.AttachedToUrl);			
			if (toUpdate == null)
				return;
			toUpdate.LastSeen = DateTime.Now;		
			if (toUpdate != null)
				Properties.DataAccess.UpdateLink (toUpdate);
		}
		private string[] GetUniqueLinksFromSite(string content){
			this.linksFound = new LinkExtract().GetElements (content,this.leaveSite,this.AttachedToUrl);
			List<string> links = new List<string> ();
			foreach (string s in linksFound) {
				if (!links.Contains (s))
					links.Add (s);
			}
			return links.ToArray ();
		}
		private void ProcessSite(){
			//Parse the child nodes
			bool isAllowed = new Robotstxt().CanBeParsed(this.AttachedToUrl);
			if (!isAllowed) {
				Output.Print (string.Format("[E]: Site {0} was not allowed", this.AttachedToUrl),true);
			} else {
				Output.Print (string.Format("[I]: Moving to {0}",this.AttachedToUrl),false);
				this.content = new Downloader ().DownloadSite (this.AttachedToUrl);	
				if (content == string.Empty)
					return;
				//harvesting data!
				this.linksFound = GetUniqueLinksFromSite (content);
				this.Images = new ImageExtract ().GetElements (content, this.leaveSite, this.AttachedToUrl);
				string[] keywords = new KeywordExtract ().GetElements (content, this.leaveSite, this.AttachedToUrl);
				//*****************************Store the child links*****************************
				//Store the link
				bool isExisting = Properties.DataAccess.IsStored (this.AttachedToUrl);
				if (!isExisting) {
					Output.Print (string.Format ("[I]: Site {0} is new, will be added to DataBase.", this.AttachedToUrl), false);
					this.NewSiteToIndex (keywords);
					this.ChildLinkProcessing (linksFound);
				}else {
					Output.Print (string.Format ("[I]: Site {0} is not new, updating last seen timestamp.", this.AttachedToUrl), false);
					Properties.AlreadyIndexed++;
					this.UpdateLastSeen (this.AttachedToUrl);
				}
			}
		}
	}
}
