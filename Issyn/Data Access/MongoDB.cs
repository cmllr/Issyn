using System;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using System.Linq;


namespace Issyn2
{
	public class MongoDB : IDataAccess
	{
		private MongoServer server;
		private MongoDatabase index;
		public MongoDB ()
		{
			server = MongoServer.Create();
			index = server.GetDatabase("index");
		}

		#region IDataAccess implementation

		public void StoreLink (Link content)
		{
			var inx = index.GetCollection<Link>("index");
			var result = inx.Insert (content);
		}

		public void UpdateLink (Link content)
		{
			if (this.GetLink (content.Target) == null)
				return;
			var inx = index.GetCollection<Link>("index");	
			Link toUpdate = this.GetLink (content.Target);	
			toUpdate.LastSeen = DateTime.Now;
			toUpdate.Text = content.Text;
			toUpdate.Parent = content.Parent;
			toUpdate.Created = content.Created;
			toUpdate.Images = content.Images;
			toUpdate.Checksum = content.Checksum;
			toUpdate.SiteContent = content.SiteContent;
			var result =  inx.Save (toUpdate);
		}

		public void DeleteLink (Link content)
		{
			throw new NotImplementedException ();
		}

		public Link[] GetLinks ()
		{
			MongoCursor cursor = index.GetCollection<Link>("index").FindAll();
			List<Link> links = new List<Link> ();
			foreach (Link l in cursor)
			{
				links.Add (l);
			}
			return links.ToArray();
		}

		public Link GetLink (Uri url)
		{	
			var link =	index.GetCollection<Link>("index").AsQueryable<Link>().First(l => l.Target == url);
			return link;
		}

		public bool IsStored (Uri url)
		{
			return (index.GetCollection<Link>("index").AsQueryable<Link>().Count(l => l.Target == url) != 0);
		}

		public void NewSiteToIndex (string[] keywords, Uri target, Uri referrer, string content, string[] childs, string[] images)
		{
			this.StoreLink (new Link () {
				Keywords = keywords,
				Target = target,
				SiteContent = content,
				Childs = childs,
				Images = images,
				Parent = new List<Uri> (){referrer },
				Checksum = System.Hash(content)
			});
		}

		public void UpdateLastSeen (Uri url)
		{
			if (this.GetLink (url) == null)
				return;
			var inx = index.GetCollection<Link>("index");	
			Link toUpdate = this.GetLink (url);	
			toUpdate.LastSeen = DateTime.Now;
			var result = inx.Save (toUpdate);
		}

		public void AddBackLinkIfNeeded (Uri site, Uri currentUrl)
		{
			Link mayBeBacklink = Index.SiteIndex.FirstOrDefault (t => t.Target == site);

			if (mayBeBacklink != null) {
				if (!mayBeBacklink.Parent.Contains (currentUrl) && mayBeBacklink.Parent.Count(l => l.ToString() == currentUrl.ToString()) == 0) {
					mayBeBacklink.Parent.Add (currentUrl);
					new MongoDB ().UpdateLink (mayBeBacklink);
				}
			}
		}

		public bool NeedsUpdate (Uri target, string newHash)
		{
			Link link = GetLink (target);			
			if (link == null)
				return true;
			if (newHash != link.Checksum)
				return true;
			else
				return false;
		}

		public string ConnectionString {
			get {
				return "";
			}
			set {

			}
		}

		#endregion

	}
}

