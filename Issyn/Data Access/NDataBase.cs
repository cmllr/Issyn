using System;
using NDatabase;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


namespace Issyn2
{
	/// <summary>
	/// The implementation of the database layer for NDataBase3
	/// </summary>
	public class NDataBaseLayer : IDataAccess
	{
		string _connectionString;
		#region IDataAccess implementation
		public string ConnectionString {
			get {
				return _connectionString;
			}
			set {
				_connectionString = value;
			}
		}

		public void StoreLink (Link content)
		{
			using (var odb = OdbFactory.Open (ConnectionString)) {
				odb.Store (content);
				if (Index.SiteIndex.Count(l => l.Target == content.Target) == 0)
					Index.SiteIndex.Add(content);
			}
		}

		public void UpdateLink (Link content)
		{
			ConnectionString = "./test.db";
			using (var odb = OdbFactory.Open (ConnectionString)) {
				if (odb.Query<Link> ().Execute<Link> ().Count (l => l.Target == content.Target) == 0)
					return;
				Link toUpdate  = odb.Query<Link>().Execute<Link>().Where(l => l.Target == content.Target).First();				
				toUpdate.LastSeen = DateTime.Now;
				toUpdate.Text = content.Text;
				toUpdate.Parent = content.Parent;
				toUpdate.Created = content.Created;
				toUpdate.Images = content.Images;
				toUpdate.Checksum = content.Checksum;
				toUpdate.SiteContent = content.SiteContent;
				odb.Store (toUpdate);
			}
		}

		public void DeleteLink (Link content)
		{
			throw new NotImplementedException ();
		}
		public bool IsStored (Uri link)
		{
			using (var odb = OdbFactory.Open (ConnectionString)) {
				return (odb.AsQueryable<Link> ().Count (l => l.Target == link) > 0) ? true : false;
			}
		}

		public Link[] GetLinks ()
		{
			using (var odb = OdbFactory.Open (ConnectionString)) {
				var links = odb.Query<Link> ().Execute<Link> ();
				return links.ToArray();
			}
		}

		public Link GetLink (Uri url)
		{
			using (var odb = OdbFactory.Open (ConnectionString)) {
				Link toReturn = odb.AsQueryable<Link> ().FirstOrDefault (l => l.Target == url);
				return toReturn;
			}		
		}
		public void NewSiteToIndex(string[] keywords,Uri target, Uri referrer,string content, string[] childs,string[] images,DateTime expires){
			RunParameters.DataAccess.StoreLink (new Link () {
				Target = target,
				Parent = new List<Uri> (){referrer },
				Text = string.Empty,
				Created = DateTime.Now,
				LastSeen = DateTime.Now,
				Keywords = keywords,
				Childs = childs,
				SiteContent = content,
				Images = images,
				Checksum = System.Hash(content),
				Expires = expires
			});
		}
		public void UpdateLastSeen(Uri url){
			Link toUpdate = RunParameters.DataAccess.GetLink (url);			
			if (toUpdate == null)
				return;
			toUpdate.LastSeen = DateTime.Now;		
			if (toUpdate != null)
				RunParameters.DataAccess.UpdateLink (toUpdate);
		}
		public void AddBackLinkIfNeeded(Uri site,Uri currentUrl){
			Link mayBeBacklink = Index.SiteIndex.FirstOrDefault (t => t.Target == site);

			if (mayBeBacklink != null) {
				if (!mayBeBacklink.Parent.Contains (currentUrl) && mayBeBacklink.Parent.Count(l => l.ToString() == currentUrl.ToString()) == 0) {
					mayBeBacklink.Parent.Add (currentUrl);
					new NDataBaseLayer ().UpdateLink (mayBeBacklink);
				}
			}
		}
		public bool NeedsUpdate (Uri target,string newHash)
		{
			Link link = GetLink (target);			
			if (link == null)
				return true;
			if (newHash != link.Checksum)
				return true;
			else
				return false;
		}
		#endregion
	}
}

