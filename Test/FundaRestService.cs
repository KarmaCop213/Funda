using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Test.Models;

namespace Test
{
	public class FundaRestService
	{
		private readonly IHttpHandler httpHandler;
		private readonly IConfigurationRoot configuration;

		public FundaRestService(IHttpHandler httpHandler, IConfigurationRoot configuration)
		{
			this.httpHandler = httpHandler;
			this.configuration = configuration;
		}

		internal async Task<FundaResponse> GetFundaResponse(string city, string amenity, int page = 1, int pageSize = 25)
		{
			string urlCity = string.IsNullOrWhiteSpace(city) ? string.Empty : "/" + city;
			string urlAmenity = string.IsNullOrWhiteSpace(amenity) ? string.Empty : "/" + amenity;
			return await Policy.Handle<HttpRequestException>().WaitAndRetryAsync(100, retryAttempt =>
			{
				Console.WriteLine($"retry : {retryAttempt}");
				return TimeSpan.FromSeconds(retryAttempt);
			}).ExecuteAsync(async () =>
			{
				using (var response = await httpHandler.GetAsync(configuration.GetValue<string>("fundaUrl")
																					.Replace("{urlCity}", urlCity)
																					.Replace("{urlAmenity}", urlAmenity)
																					.Replace("{page}", page.ToString())
																					.Replace("{pageSize}", pageSize.ToString())))
				{
					if (!response.IsSuccessStatusCode)
						throw new HttpRequestException();
					string apiResponse = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<FundaResponse>(apiResponse);
				}
			});
		}
	}
}
