namespace RegComparator
{
    internal class UnmatchedDocMarker : IUnmatchedDocMarker
    {
        private List<Document> _docList1CDO;
        private List<Document> _docListReg;

        enum UnmatchedField
        {
            Date = 0,
            Number = 1,
            Salary = 2,
            None = 3
        }

        internal UnmatchedDocMarker(List<Document> docList1CDO, List<Document> docListReg)
        {
            _docList1CDO = docList1CDO;
            _docListReg = docListReg;
        }

        public void MarkDocuments()
        {
            _docList1CDO.ForEach(FindDocumentSetComment);
        }

        private void FindDocumentSetComment(Document documentDo)
        {
            foreach (Document documentUpp in _docListReg)
            {
                bool isDocumentMatch = CompareSingleDocuments(documentDo, documentUpp);
                if (isDocumentMatch) break;
            }
        }

        private bool CompareSingleDocuments(Document documentDo, Document documentUpp)
        {
            
            bool isDateMatch = documentDo.Date == documentUpp.Date;
            bool isNumberMatch = documentDo.Number == documentUpp.Number;
            bool isSalaryMatch = documentDo.Salary == documentUpp.Salary;
            bool isTypeMatch = documentDo.Type == documentUpp.Type;
            bool isDocumentMatched = false;
            int stylePosition = new();

            int numberOfMatch = new();

            if (isDateMatch) 
                numberOfMatch++;
            if (isNumberMatch) 
                numberOfMatch++;
            if (isSalaryMatch) 
                numberOfMatch++;
            if (isTypeMatch) 
                numberOfMatch++;

            UnmatchedField unmatchedField = UnmatchedField.None;

            if (numberOfMatch == 3) 
            {
                isDocumentMatched = true;
                if (!isDateMatch) 
                {
                    unmatchedField = UnmatchedField.Date;
                    stylePosition = 4;
                } 
                if (!isNumberMatch)
                {
                    unmatchedField = UnmatchedField.Number;
                    stylePosition = 5;

                }
                if (!isSalaryMatch)
                {
                    unmatchedField = UnmatchedField.Salary;
                    stylePosition = 6;
                }
            }

            SetComment(documentDo, documentUpp, unmatchedField);
            SetStylePosition(documentDo, stylePosition);

            return isDocumentMatched;
        }

        private void SetStylePosition(Document documentDo, int stylePosition)
        {
            documentDo.StylePosition = stylePosition;
        }

        private void SetComment(Document documentDo, Document documentUpp, UnmatchedField unmatchedField)
        {
            switch(unmatchedField)
            {
                case UnmatchedField.Date:
                    documentDo.Comment = $"Дата: {documentUpp.Date}";
                    break;
                case UnmatchedField.Number:
                    documentDo.Comment = $"Номер: {documentUpp.Number}";
                    break;
                case UnmatchedField.Salary:
                    documentDo.Comment = $"Сумма: {documentUpp.Salary.ToString()}";
                    break;
                case UnmatchedField.None:
                    documentDo.Comment = "Документ не найден в реестре";
                    break;
            }
        }
    }
}
