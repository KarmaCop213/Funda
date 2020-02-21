using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Models
{
	public class FundaResponse
	{
		public IEnumerable<FundaObject> Objects { get; set; }
		public FundaPaging Paging { get; set; }
		public int TotaalAantalObjecten { get; set; }
	}

	public class FundaObject
	{
		public int MakelaarId { get; set; }
		public string MakelaarNaam { get; set; }
	}

	public class FundaPaging
	{
		public int AantalPaginas { get; set; }
		public int HuidigePagina { get; set; }
	}


}
