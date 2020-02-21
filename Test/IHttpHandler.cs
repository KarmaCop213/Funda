using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
	public interface IHttpHandler
	{
		HttpResponseMessage Get(string url);
		HttpResponseMessage Post(string url, HttpContent content);
		Task<HttpResponseMessage> GetAsync(string url);
		Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
	}
}
