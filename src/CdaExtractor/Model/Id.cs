using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class Id
    {
        public string Root { get; set; }
        public string Extension { get; set; }

        public override string ToString()
        {
            var root = Root ?? "";
            var extension = Extension ?? "";

            return root + "^" + extension;

        }
    }
}
