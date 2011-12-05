//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;
using NUnit.Framework;
using Rhino.Mocks;
using Tp.Integration.Ide.VisualStudio.UI;
using Is=Rhino.Mocks.Constraints.Is;

namespace Tp.Integration.Ide.VisualStudio
{
	/// <summary>
	/// Prepares fake DTE environment and tests addin in it.
	/// </summary>
	[TestFixture]
	public class ConnectTest
	{
		[Test]
		public void TestUiSetupModeConnect()
		{
			var connect = new Connect();
			Array array = null;
			connect.OnConnection(null, ext_ConnectMode.ext_cm_UISetup, null, ref array); // Do nothing.
			connect.OnDisconnection(ext_DisconnectMode.ext_dm_UISetupComplete, ref array); // Do nothing.
		}

		[Test]
		[Explicit]
		public void TestStartupModeConnect()
		{
			var mocks = new MockRepository();

			var form = new Form(); // Visual Studio main window.
			var toolWindowControl = new ToolWindowControl {Dock = DockStyle.Fill}; // To Do tool window control.
			form.Controls.Add(toolWindowControl);
			DTE2 application;
			AddIn addin;

			using (mocks.Record())
			{
				MockDte(mocks, form, toolWindowControl, out application, out addin);
			}

			var settings = new Settings
			               	{
			               		Uri = new Uri("http://localhost/tp2"),
			               		Login = "admin",
			               		DecryptedPassword = "admin",
			               		AutoLogin = true,
			               		AutoRefresh = false,
			               	};
			settings.Save();

			using (mocks.Playback())
			{
				Array array = null;

				// Create addin instance.
				var connect = new Connect();

				Console.WriteLine("Test started");

				// Display Visual Studio main window.
				form.Show();

				// Connect addin instance to Visual Studio.
				connect.OnConnection(application, ext_ConnectMode.ext_cm_Startup, addin, ref array);
				connect.OnStartupComplete(ref array);

				// Check command statuses.
				vsCommandStatus status = vsCommandStatus.vsCommandStatusUnsupported;
				object tmp = null;

				connect.QueryStatus(addin.ProgID + "." + Connect.CmdLogin, vsCommandStatusTextWanted.vsCommandStatusTextWantedNone,
				                    ref status, ref tmp);
				Assert.AreEqual(vsCommandStatus.vsCommandStatusSupported, status);

				connect.QueryStatus(addin.ProgID + "." + Connect.CmdLogout, vsCommandStatusTextWanted.vsCommandStatusTextWantedNone,
				                    ref status, ref tmp);
				Assert.AreEqual(vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled, status);

				connect.QueryStatus(addin.ProgID + "." + Connect.CmdToDoList,
				                    vsCommandStatusTextWanted.vsCommandStatusTextWantedNone, ref status, ref tmp);
				Assert.AreEqual(vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled, status);

				connect.QueryStatus(addin.ProgID + "." + Connect.CmdOptions, vsCommandStatusTextWanted.vsCommandStatusTextWantedNone,
				                    ref status, ref tmp);
				Assert.AreEqual(vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled, status);

				// Execute some commands.
				bool handled = false;
				connect.Exec(addin.ProgID + "." + Connect.CmdToDoList, vsCommandExecOption.vsCommandExecOptionDoDefault, ref tmp,
				             ref tmp, ref handled);
				Assert.IsTrue(handled);

				form.Closed += (sender, e) =>
				               	{
				               		// Disconnect addin instance from Visual Studio.
				               		connect.OnDisconnection(ext_DisconnectMode.ext_dm_HostShutdown, ref array);
				               		Console.WriteLine("Test ended");
				               	};
				Application.Run(form);
			}
		}

		/// <summary>
		/// Prepares fake DTE environment and AddIn object.
		/// </summary>
		private static void MockDte(MockRepository mocks, IWin32Window form, ToolWindowControl toolWindowControl,
		                            out DTE2 application, out AddIn addin)
		{
			object programmableObject = null;

			application = mocks.DynamicMock<DTE2>();
			addin = mocks.DynamicMock<AddIn>();
			var windows = mocks.DynamicMock<Windows2>();
			var window = mocks.DynamicMock<Window>();
			var toolsWindows = mocks.DynamicMock<ToolWindows>();
			var outputWindow = mocks.DynamicMock<OutputWindow>();
			var outputWindowPanes = mocks.DynamicMock<OutputWindowPanes>();
			var outputWindowPane = mocks.DynamicMock<OutputWindowPane>();
			var commands = mocks.DynamicMock<Commands2>();
			var commandBars = mocks.DynamicMock<CommandBars>();
			var commandBar = mocks.DynamicMock<CommandBar>();
			var commandBarControls = mocks.DynamicMock<CommandBarControls>();
			var commandBarControl = mocks.DynamicMock<CommandBarControl>();
			var commandBarPopup = mocks.DynamicMock<CommandBarPopup>();

			Expect.Call(addin.ProgID).Return("Tp.Integration.Ide.VisualStudio").Repeat.Any();
			Expect.Call(application.LocaleID).Return(1033).Repeat.Any();
			Expect.Call(application.Windows).Return(windows).Repeat.Any();
			Expect.Call(application.MainWindow).Return(window).Repeat.Any();
			Expect.Call(application.ToolWindows).Return(toolsWindows).Repeat.Any();
			Expect.Call(toolsWindows.OutputWindow).Return(outputWindow).Repeat.Any();
			Expect.Call(outputWindow.Parent).Return(window).Repeat.Any();
			Expect.Call(outputWindow.OutputWindowPanes).Return(outputWindowPanes).Repeat.Any();
			Expect.Call(outputWindowPanes.Item("TargetProcess")).Return(outputWindowPane).Repeat.Any();
			Expect.Call(application.Commands).Return(commands).Repeat.Any();
			Expect.Call(application.CommandBars).Return(commandBars).Repeat.Any();
			Expect.Call(commandBars["MenuBar"]).Return(commandBar).Repeat.Any();
			Expect.Call(commandBar.Controls).Return(commandBarControls).Repeat.Any();
			Expect.Call(commandBarControls["Window"]).Return(commandBarControl).Repeat.Any();
			Expect.Call(commandBarControls.Add(null, null, null, null, null)).Return(commandBarPopup).Constraints(Is.Anything(),
			                                                                                                      Is.Anything(),
			                                                                                                      Is.Anything(),
			                                                                                                      Is.Anything(),
			                                                                                                      Is.Anything()).
				Repeat.Any();

			// Set up tool window factory.
			Expect.Call(windows.CreateToolWindow2(addin, typeof (ToolWindowControl).Assembly.Location,
			                                      typeof (ToolWindowControl).FullName, "TargetProcess Window",
			                                      "{A523C4AD-EA9B-4944-94AB-41313992B2EE}", ref programmableObject)).OutRef(
				toolWindowControl).Return(window).Repeat.Any();
			// Specify Visual Studio main window handle.
			Expect.Call(window.HWnd).Return((int) form.Handle).Repeat.Any();
			// Redirect addin logging from output window pane to console.
			Expect.Call(() => { outputWindowPane.OutputString(null); }).Constraints(Is.Anything()).Repeat.Any().Do(
				(Action<String>) (s => { Console.WriteLine(s); }));
		}
	}
}