//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.IO;

namespace Tp.Integration.Ide.VisualStudio.Utils
{
	internal static class UriHelper
	{
		public static Uri ViewEntityUri(Uri baseUri, int assignableID)
		{
			var builder = new UriBuilder(baseUri);
			builder.Path = Path.Combine(builder.Path, "View.aspx");
			builder.Query = string.Format("id={0}", assignableID);
			builder.Fragment = null;
			return builder.Uri;
		}

		public static Uri WebServiceUri(Uri baseUri, string webServiceName)
		{
			var builder = new UriBuilder(baseUri);
			builder.Path = Path.Combine(builder.Path, string.Format("Services/{0}.asmx", webServiceName));
			builder.Query = null;
			builder.Fragment = null;
			return builder.Uri;
		}
	}
}