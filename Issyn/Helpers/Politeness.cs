using System;
using System.Net;
using System.Linq;
namespace Issyn2
{
	/// <summary>
	/// The politeness module of the crawler. Checks the given http status code if it the crawler can accept it.
	/// </summary>
	public class Politeness
	{
		/// <summary>
		/// The accepted Status codes.
		/// </summary>
		HttpStatusCode[] _accepted;
		/// <summary>
		/// Initializes a new instance of the <see cref="Issyn2.Politeness"/> class.
		/// </summary>
		/// <param name="accepted"> The accepted Status codes.</param>
		public Politeness (HttpStatusCode[] accepted)
		{
			this._accepted = accepted;
		}
		/// <summary>
		/// Determines if the given status code should be accepted.
		/// </summary>
		/// <returns><c>true</c> if this instance is O the specified actual; otherwise, <c>false</c>.</returns>
		/// <param name="actual">Actual.</param>
		public bool IsOK(HttpStatusCode actual){
			if (_accepted.Count (s => s == actual) == 0) {
				return false;
			} else {
				return true;
			}
		}
	}
}

