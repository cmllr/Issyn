using System;
using System.Net;

namespace Issyn2
{
	public static class RunParameters
	{
		public static int RequestCount = 0;
		public static string Robotstxt = string.Empty;
		public static bool WasSiteMapParsed = false;
		public static int InIndex = 0;
		public static int CurrentFails = 0;
		public static IDataAccess DataAccess ;
		public static HttpStatusCode[] AcceptedErrorStatus = new HttpStatusCode[]{HttpStatusCode.Continue,HttpStatusCode.Found,HttpStatusCode.Moved,HttpStatusCode.MovedPermanently,HttpStatusCode.NotFound};
	}
}

