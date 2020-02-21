using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Models;

namespace Test
{
	class FundaService
	{
		private readonly FundaRestService fundaRestService;

		public FundaService(FundaRestService fundaRestService)
		{
			this.fundaRestService = fundaRestService;
		}

		public async Task CreateReport(string city, string amenity = null)
		{
			var fundaResponse = await fundaRestService.GetFundaResponse(city, amenity);
			var results = (List<FundaObject>)fundaResponse.Objects;
			for (int i = 2; i <= fundaResponse.Paging.AantalPaginas; i++)
			{
				fundaResponse = await fundaRestService.GetFundaResponse(city, amenity, i);
				results.AddRange(fundaResponse.Objects);
			}
			// on the next line, since I'm assuming different ids can have the same name and the same id can have different names,
			// I'm creating a dictionary with id - name pairs, being the name the first name found for that id.
			var makelaarsDic = results.GroupBy(o => o.MakelaarId).Select(o => o.First()).ToDictionary(o => o.MakelaarId, o => o.MakelaarNaam);
			var orderedResults = results.GroupBy(o => o.MakelaarId).Select(o => new { MakelaarNaam = makelaarsDic[o.Key], Count = o.Count() }).OrderByDescending(o => o.Count);
			var maxNameLength = orderedResults.Take(10).OrderByDescending(o => o.MakelaarNaam.Length).First().MakelaarNaam.Length;
			Console.WriteLine($"{"MakelaarNaam".PadRight(maxNameLength, ' ')}: Count");
			foreach (var orderedResult in orderedResults.Take(10))
			{
				Console.WriteLine($"{orderedResult.MakelaarNaam.PadRight(maxNameLength, ' ')}: {orderedResult.Count}");
			}
		}
	}
}
