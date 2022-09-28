
namespace CheckDocumentRegistry
{
    public class PartialDocumentsComparator
    {
        public List<Document> DocumentsDo;
        public List<Document> DocumentsUpp;

        enum UnmatchedField
        {
            Date,
            Number,
            Salary,
            None
        }

        public PartialDocumentsComparator(List<Document> documentsDo, List<Document> documentsUpp)
        {
            DocumentsDo = documentsDo;
            DocumentsUpp = documentsUpp;
        }

        public void CommentUnmatchedDocuments()
        {
            this.DocumentsDo.ForEach(FindDocumentSetComment);
        }

        private void FindDocumentSetComment(Document documentDo)
        {
            foreach (Document documentUpp in this.DocumentsUpp)
            {
                bool isDocumentMatch = this.CompareSingleDocuments(documentDo, documentUpp);
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

            int numberOfMatch = new();

            if (isDateMatch) numberOfMatch++;
            if (isNumberMatch) numberOfMatch++;
            if (isSalaryMatch) numberOfMatch++;
            if (isTypeMatch) numberOfMatch++;

            UnmatchedField unmatchedField = UnmatchedField.None;

            if (numberOfMatch == 3) 
            {
                isDocumentMatched = true;
                if (!isDateMatch) unmatchedField = UnmatchedField.Date;
                if (!isNumberMatch) unmatchedField = UnmatchedField.Number;
                if (!isSalaryMatch) unmatchedField = UnmatchedField.Salary;
            }

            this.SetComment(documentDo, documentUpp, unmatchedField);

            return isDocumentMatched;
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
                    documentDo.Comment = "Докумен не найден в УПП";
                    break;
            }
        }
    }
}
