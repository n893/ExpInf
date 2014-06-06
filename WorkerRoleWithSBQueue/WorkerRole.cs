using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using DAL;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WorkerRoleWithSBQueue
{
	public class WorkerRole : RoleEntryPoint
	{
		private readonly UserRepository _userRepo = new UserRepository();

		// The name of your queue
		const string QueueName = "UpdQueue";

		// QueueClient is thread-safe. Recommended that you cache 
		// rather than recreating it on every request
		QueueClient _client;
		private CloudBlockBlob _blob;
		ManualResetEvent CompletedEvent = new ManualResetEvent(false);

		public override void Run()
		{
			Trace.WriteLine("Starting processing of messages");

			while (true)
			{
				var receivedMessages = _client.ReceiveBatch(32);
				var brokeredMessages = receivedMessages as BrokeredMessage[] ?? receivedMessages.ToArray();
				if (!brokeredMessages.Any())
				{
					Thread.Sleep(1*60*1000);
					continue;
				}
				// Посчитать лучшего
				var bestUserName = _userRepo.GetTheBest();
				// Взять лучшего из блоба
				string blobText;
				if (_blob.Exists())
				{
					// Считать блоб
					blobText = _blob.DownloadText();
					var parts = blobText.Split(',');
					if (parts[0] != bestUserName)
					{
						// Записать нового лучшего пользователя, указав текущую дату
						uploadBlob(bestUserName);
					}
				}
				else
				{
					// Если блоб еще пустой
					uploadBlob(bestUserName);
				}
				// Удалить все сообщения
				foreach (var receivedMessage in brokeredMessages)
				{
					receivedMessage.Complete();
				}
			}
			// Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
			//_client.OnMessage((receivedMessage) =>
			//	{
			//		try
			//		{
			//			// Process the message
			//			Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
			//		}
			//		catch
			//		{
			//			// Handle any message processing specific exceptions here
			//		}
			//	});

			//CompletedEvent.WaitOne();
		}

		private void uploadBlob(string user)
		{
			_blob.UploadText(string.Join(",", user, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
		}

		public override bool OnStart()
		{
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;

			// Create the queue if it does not exist already
			string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
			var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
			if (!namespaceManager.QueueExists(QueueName))
			{
				namespaceManager.CreateQueue(QueueName);
			}
			// Initialize the connection to Service Bus Queue
			_client = QueueClient.CreateFromConnectionString(connectionString, QueueName);

			var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
			// Create the blob client.
			var blobClient = storageAccount.CreateCloudBlobClient();
			// Retrieve a reference to a container. 
			var container = blobClient.GetContainerReference("mycontainer");
			// Create the container if it doesn't already exist.
			container.CreateIfNotExists();
			_blob = container.GetBlockBlobReference("myblob");

			return base.OnStart();
		}

		public override void OnStop()
		{
			// Close the connection to Service Bus Queue
			_client.Close();
			CompletedEvent.Set();
			base.OnStop();
		}
	}
}
