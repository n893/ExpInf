using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading;
using DAL;
using Microsoft.WindowsAzure.ServiceRuntime;
using SendGrid;

namespace WorkerRoleA
{
	public class WorkerRole : RoleEntryPoint
	{
		private readonly UserRepository _userRepo = new UserRepository();

		public override void Run()
		{
			// This is a sample worker implementation. Replace with your logic.
			Trace.TraceInformation("WorkerRoleA entry point called");

			while (true)
			{
				Trace.TraceInformation("Working");
				var users = _userRepo.GetBirthdayUsers();
				sendEmails(users);
				// Wait for 1 day
				Thread.Sleep(24*60*60*1000);
			}
		}

		public override bool OnStart()
		{
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			return base.OnStart();
		}

		private void sendEmails(Dictionary<string, string> users)
		{
			if (users.Count < 1)
			{
				return;
			}
			var credentials = new NetworkCredential("azure_f8806f6cc6e59f1429384a7d6642115e@azure.com", "qYzcc6C3Z1A7TB2");
			var transportWeb = new Web(credentials);
			var myMessage = new SendGridMessage();
			myMessage.From = new MailAddress("azure_f8806f6cc6e59f1429384a7d6642115e@azure.com", "ExpInfo team");
			myMessage.Text = "Happy Birthday! Thanks for using our servises!";
			foreach (var user in users)
			{
				try
				{
					myMessage.AddTo(user.Value);
					myMessage.Subject = string.Format("Hello, {0}!", user.Key);
					// Send the email.
					transportWeb.Deliver(myMessage);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
