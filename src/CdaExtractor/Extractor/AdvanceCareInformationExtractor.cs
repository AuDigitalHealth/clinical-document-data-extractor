using Nehta.VendorLibrary.CdaExtractor.Model;
using System;
using System.Collections.Generic;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
	public class AdvanceCareInformationExtractor : ICdaExtractor<AdvanceCareInformation>
	{
		private readonly IDictionary<string, string> _documentXPaths;

		public AdvanceCareInformationExtractor(IDictionary<string, string> documentXPaths)
		{
			this._documentXPaths = documentXPaths;
		}

		public AdvanceCareInformation Extract(CdaXmlDocument cdaDocument)
		{
			PersonName documentAuthorPersonName = null;
			PersonName personName = cdaDocument.GetPersonName(this._documentXPaths.Get("DocumentAuthorPersonName"));
			bool flag = personName != null;
			if (flag)
			{
				documentAuthorPersonName = personName;
			}
			return new AdvanceCareInformation
			{
				DocumentAuthorPersonName = documentAuthorPersonName
			};
		}
	}
}
