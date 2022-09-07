using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsComparator
{
    public class DoDocument
    {
        int files;
        int tasks;
        int links;
        int importance;
        string title;
        string type;
        string theme;
        string regNumber;
        string creator;
        string signed;
        string date;

        DoDocument (string[] returedString) 
        {
            this.files = Parse(returedString[0]); 
            int tasks;
            int links;
            int importance;
            string title;
            string type;
            string theme;
            string regNumber;
            string creator;
            string signed;
            string date;
        }
        

    }
}
