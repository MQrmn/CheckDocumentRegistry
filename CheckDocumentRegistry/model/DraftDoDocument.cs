using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsComparator
{
    public class DraftDoDocument
    {
        public int files;
        public int tasks;
        public int links;
        public int importance;
        public string title;
        public string type;
        public string theme;
        public string regNumber;
        public string creator;
        public string signed;
        public string date;

        public DraftDoDocument(string[] returedString)
        {

            if (returedString[0] != null)
                this.files = Int32.Parse(returedString[0]);
            if (returedString[1] != null)
                this.tasks = Int32.Parse(returedString[1]);
            if (returedString[2] != null)
                this.links = Int32.Parse(returedString[2]);
            if (returedString[3] != null)
                this.importance = Int32.Parse(returedString[3]);
            this.title = returedString[4];
            this.type = returedString[5];
            this.theme = returedString[6];
            this.regNumber = returedString[7];
            this.creator = returedString[8];
            this.signed = returedString[9];
            this.date = returedString[10];
        }


    }
}
