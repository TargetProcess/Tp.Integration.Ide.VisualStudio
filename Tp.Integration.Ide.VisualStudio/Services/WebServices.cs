// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Net;
using System.Text;
using Microsoft.Web.Services3;
using Tp.AssignableServiceProxy;
using Tp.AuthenticationServiceProxy;
using Tp.Integration.Ide.VisualStudio.Utils;
using Tp.MyAssignmentsServiceProxy;
using Tp.Service.Proxies;

namespace Tp.Integration.Ide.VisualStudio.Services
{
	/// <summary>
	/// Makes it possible to substitute WS references with mocks for unit testing.
	/// </summary>
	internal interface IWebServices : IDisposable
	{
		void Authenticate();

		/// <summary>
		/// Gets collection of items for the To Do list.
		/// </summary>
		/// <returns>A collection of items for the To Do list.</returns>
		MyAssignments GetMyAssigments();

		/// <summary>
		/// Changes state of the To Do list item.
		/// </summary>
		/// <param name="assignable">The assignable item whose state to change.</param>
		/// <param name="entityState">The new state for the specified assignable item.</param>
		void ChangeState(AssignableSimpleDTO assignable, EntityStateDTO entityState);

		/// <summary>
		/// Sends recorded times to server.
		/// </summary>
		/// <param name="times">Array of time records to submit.</param>
		/// <returns>An array of error messages.</returns>
		SubmitTimeError[] SubmitTime(TimeSimpleDTO[] times);

		/// <summary>
		/// Gets array of user stories, tasks and bugs from the server with the specified identifiers.
		/// </summary>
		/// <param name="id">Identifiers of items to get.</param>
		/// <returns>An array of user stories, tasks and bugs from the server with the specified identifiers.</returns>
		AssignableDTO[] GetAssignables(params int[] id);
	}

	/// <summary>
	/// Creates new <see cref="IWebServices"/> instances on demand.
	/// </summary>
	internal interface IWebServicesFactory
	{
		/// <summary>
		/// Creates new <see cref="IWebServices"/> instance using connection params from the specified settings.
		/// </summary>
		/// <param name="settings">Settings with connection params.</param>
		/// <returns>A new <see cref="IWebServices"/> instance.</returns>
		IWebServices CreateWebServices(Settings settings);
	}

	/// <summary>
	/// Creates new <see cref="WebServices"/> instances.
	/// </summary>
	internal class WebServicesFactory : IWebServicesFactory
	{
		public static readonly IWebServicesFactory Instance = new WebServicesFactory();

		#region IWebServicesFactory Members

		public IWebServices CreateWebServices(Settings settings)
		{
			return new WebServices(settings);
		}

		#endregion
	}

	/// <summary>
	/// Implements interface using generated Web Service proxies.
	/// </summary>
	internal sealed class WebServices : IWebServices
	{
		private readonly Uri _baseUri;
		private readonly ICredentials _credentials;
		private readonly bool _integratedSecurity;

		private MyAssignmentsService _myAssignmentsService;
		private AssignableService _assignableService;

		public WebServices(Settings settings)
		{
			if (settings.Uri == null)
			{
				throw new ArgumentException("Base uri is empty");
			}
			if (settings.Uri.Scheme != "http" && settings.Uri.Scheme != "https")
			{
				throw new ArgumentException(string.Format("Uri scheme '{0}' not supported", settings.Uri.Scheme));
			}
			_baseUri = settings.Uri;
			_credentials = settings.Credentials;
			_integratedSecurity = settings.UseWindowsLogin;
		}

		private T GetService<T>(string name) where T : WebServicesClientProtocol, new()
		{
			var service = new T
							{
								Url = UriHelper.WebServiceUri(_baseUri, name).ToString(),
								Credentials = _credentials
							};
			var networkCredentials = (NetworkCredential) _credentials;
			CredentialInitializer.InitCredentials(service, _integratedSecurity, networkCredentials.UserName,
			                                      networkCredentials.Password);
			return service;
		}

		#region IWebServices Members

		public void Authenticate()
		{
			var authenticationService = GetService<AuthenticationService>("AuthenticationService");
			authenticationService.Authenticate();
		}

		public MyAssignments GetMyAssigments()
		{
			return MyAssignmentsService.GetMyAssigments();
		}

		public void ChangeState(AssignableSimpleDTO assignable, EntityStateDTO entityState)
		{
			MyAssignmentsService.ChangeState(assignable.AssignableID.GetValueOrDefault(),
			                                 entityState.EntityStateID.GetValueOrDefault());
		}

		public SubmitTimeError[] SubmitTime(TimeSimpleDTO[] times)
		{
			return MyAssignmentsService.SubmitTime(times);
		}

		public AssignableDTO[] GetAssignables(params int[] id)
		{
			if (id.Length == 0)
			{
				return new AssignableDTO[] {};
			}
			var s = new StringBuilder();
			s.Append("select Assignable from Assignable as Assignable where Assignable.AssignableID in (");
			var n = 0;
			foreach (var item in id)
			{
				if (n > 0)
				{
					s.Append(",");
				}
				s.Append(item);
				n++;
			}
			s.Append(")");
			return AssignableService.Retrieve(s.ToString(), new object[] {});
		}

		#endregion

		private MyAssignmentsService MyAssignmentsService
		{
			get { return _myAssignmentsService ?? (_myAssignmentsService = GetService<MyAssignmentsService>("MyAssignmentsService")); }
		}

		private AssignableService AssignableService
		{
			get { return _assignableService ?? (_assignableService = GetService<AssignableService>("AssignableService")); }
		}

		public void Dispose()
		{
			//
		}
	}
}