﻿using System;

namespace Issyn2
{
	public interface IExtractModule
	{
		string[] GetElements(string content,bool addForeign,Uri root);
	}
}
