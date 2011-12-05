// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using NUnit.Framework;

namespace Tp.Integration.Ide.VisualStudio.Utils
{
	[TestFixture]
	public class UriHelperTest
	{
		[Test]
		public void TestGetVewEntityUri()
		{
			Assert.AreEqual(
				new Uri("http://localhost/TargetProcess/View.aspx?id=1"),
				UriHelper.ViewEntityUri(new Uri("http://localhost/TargetProcess"), 1));
		}

		[Test]
		public void TestGetWebServiceUri()
		{
			Assert.AreEqual(
				new Uri("http://localhost/Services/TaskService.asmx"),
				UriHelper.WebServiceUri(new Uri("http://localhost"), "TaskService"));
			Assert.AreEqual(
				new Uri("http://localhost/TargetProcess/Services/TaskService.asmx"),
				UriHelper.WebServiceUri(new Uri("http://localhost/TargetProcess"), "TaskService"));
			Assert.AreEqual(
				new Uri("http://localhost/TargetProcess/Services/TaskService.asmx"),
				UriHelper.WebServiceUri(new Uri("http://localhost/TargetProcess/"), "TaskService"));
		}
	}
}