using System;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;

namespace ExperienceInfo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			ViewBag.TopUser = "";
			// Get Top-user from the blob
	        var storageAccount = CloudStorageAccount.Parse(
		        "DefaultEndpointsProtocol=https;AccountName=expinfoservicestorage;AccountKey=iwJt8kBV1mPCx/Af61RYfUYPasxLas61YctVZWwyOfWCFIJAkWjIwXhmgfzVmVLVjRuBZpoam9YEIEQVbj0FGA==");
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference("mycontainer");
	        if (container.Exists())
	        {
		        var blob = container.GetBlockBlobReference("myblob");
		        if (blob.Exists())
		        {
			        try
			        {
				        var text = blob.DownloadText().Split(',');
				        var topDate = new DateTime(int.Parse(text[1]), int.Parse(text[2]), int.Parse(text[3]));
				        var days = (int)((DateTime.Now - topDate).TotalDays) + 1;
				        ViewBag.TopUser = text[0];
				        ViewBag.TopDays = days;
			        }
			        catch
			        {
				        // Do nothing
			        }
		        }
	        }
			// Create the container if it doesn't already exist.
			container.CreateIfNotExists();
			
            return View();
        }
    }
}
