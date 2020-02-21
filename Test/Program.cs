using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test
{
	   	 
	class Program
	{
		static async Task Main(string[] args)
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

			IConfigurationRoot configuration = builder.Build();

			Console.WriteLine(configuration.GetConnectionString("Storage"));

			var fundaRestService = new FundaRestService(new HttpClientHandler(), configuration);
			var fundaService = new FundaService(fundaRestService);
			Console.WriteLine("Determine which makelaar's in Amsterdam have the most object listed for sale:");
			await fundaService.CreateReport("amsterdam");
			Console.WriteLine();
			Console.WriteLine("Do the same thing but only for objects with a tuin which are listed for sale.");
			await fundaService.CreateReport("amsterdam", "tuin");
			Console.WriteLine();
			Console.WriteLine("Finished, press any key to exit");
			Console.ReadLine();
		}


	}
}
