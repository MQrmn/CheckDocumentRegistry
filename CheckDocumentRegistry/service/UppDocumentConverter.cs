using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry.service
{
    internal class UppDocumentConverter
    {
        public List<UppDocument> ConvertArrToDocuments(string[][] input)
        {
            List<UppDocument> result = new List<UppDocument>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                result.Add(new UppDocument(input[i]));
            }

            return result;
        }
    }
}
