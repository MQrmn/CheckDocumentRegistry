using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    public class DoDocumentConverter
    {
        public List<DoDocument> ConvertArrToDocuments(string[][] input)
        {
            List<DoDocument> result = new List<DoDocument>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                result.Add(new DoDocument(input[i]));
            }

            return result;
        }
    }
}
