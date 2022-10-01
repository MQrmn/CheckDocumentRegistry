﻿
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CUpp : Document
    {

        public Document1CUpp(string[] document, int[] docFieldIndex)
        {
            this.Type = this.GetDocType(document[docFieldIndex[0]]);
            this.Title = document[docFieldIndex[1]];
            this.Date = document[docFieldIndex[2]];
            this.Counterparty = document[docFieldIndex[3]];
            this.Number = this.SetDocNumber(document[docFieldIndex[4]]);
            this.Company = document[docFieldIndex[5]];

            if (document[docFieldIndex[6]] != String.Empty)
                this.Salary = this.GetDocSum(document[docFieldIndex[6]]);
        }

        private protected int GetDocType(string input)
        {
            return input switch
            {
                "Да Поступление товаров и услуг" => 1,
                "Да Счет-фактура полученный" => 2,
                _ => 0
            };
        }

        private protected float GetDocSum(string stringSum)
        {
            string pattern = @"[\.]";
            string regexResult = Regex.Replace(stringSum, pattern, ",", RegexOptions.IgnoreCase);
            try
            {
                return float.Parse(regexResult);
            }
            catch
            {
                return 0;
            }
        }


    }
}