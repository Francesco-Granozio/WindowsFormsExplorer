using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsExplorer.Domain
{
    public class FormInfo
    {
        public string Name { get; set; }
        
        public string Type { get; set; }
        
        public string Text { get; set; }

        public bool Visible { get; set; }

        public int Handle { get; set; }
    }
}
