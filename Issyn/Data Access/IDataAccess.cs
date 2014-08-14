using System;

namespace Issyn2
{
	public interface IDataAccess
	{
		string ConnectionString { get; set;}
		void StoreLink(Link content);
		void UpdateLink(Link content);
		void DeleteLink(Link content);
		Link[] GetLinks();
		Link GetLink(Uri url);
		bool IsStored(Uri url);
	}
}

