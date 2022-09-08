﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsComparator
{
    public class DoDocument
    {
        public string docType { get; }
        public string docTitle { get; }
        public string docCompany { get; }
        public string docCounterparty { get; }
        public string docNumber { get; }
        public string docDate { get; }
        public string docSum { get; }
        public bool isUpd { get; }


        public DoDocument(string[] docValues)
        {

            this.docType = docValues[0];
            this.docTitle = docValues[1];
            this.docCompany = docValues[2];
            this.docCounterparty = docValues[3];
            this.docNumber = docValues[4];
            this.docDate = docValues[5];
            this.docSum = docValues[6];
            if (docValues[7] != null) this.isUpd = true;

        }

        

    }
}