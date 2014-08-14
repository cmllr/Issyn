using System;
using NDatabase;
using System.Linq;
using System.Collections.Generic;


namespace Issyn2
{
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
		#endregion
	}
}

